using Microsoft.AspNetCore.Mvc;
using Brisqueometer.Models;
using Luxoria.Algorithm.BrisqueScore;

namespace Brisqueometer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public HomeController(
        ILogger<HomeController> logger,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _configuration = configuration;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(ImageUploadViewModel model)
    {
        if (model.ImageFile == null || model.ImageFile.Length == 0)
        {
            ModelState.AddModelError("", "Please select an image file.");
            return View("Index");
        }

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
        var extension = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError("", "Only image files (.jpg, .jpeg, .png, .bmp) are allowed.");
            return View("Index");
        }

        // Check file size
        var maxFileSize = _configuration.GetValue<long>("BrisqueSettings:MaxFileSizeBytes", 10485760);
        if (model.ImageFile.Length > maxFileSize)
        {
            ModelState.AddModelError("", $"File size cannot exceed {maxFileSize / 1024 / 1024} MB.");
            return View("Index");
        }

        try
        {
            // Create uploads directory if it doesn't exist
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            // Save the uploaded file
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }

            // Compute BRISQUE score
            var modelPath = Path.Combine(_environment.ContentRootPath, 
                _configuration["BrisqueSettings:ModelPath"] ?? "Models/brisque_model_live.yml");
            var rangePath = Path.Combine(_environment.ContentRootPath, 
                _configuration["BrisqueSettings:RangePath"] ?? "Models/brisque_range_live.yml");

            double score;
            using (var brisque = new BrisqueInterop(modelPath, rangePath))
            {
                score = brisque.ComputeScore(filePath);
            }

            // Create result view model
            var result = new BrisqueResultViewModel
            {
                Score = score,
                ImagePath = $"/uploads/{fileName}",
                FileName = model.ImageFile.FileName,
                QualityDescription = GetQualityDescription(score)
            };

            return View("Result", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing image");
            ModelState.AddModelError("", $"Error processing image: {ex.Message}");
            return View("Index");
        }
    }

    private static string GetQualityDescription(double score)
    {
        // BRISQUE scores typically range from 0 to 100
        // Lower scores indicate better quality
        return score switch
        {
            < 20 => "Excellent",
            < 40 => "Good",
            < 60 => "Fair",
            < 80 => "Poor",
            _ => "Very Poor"
        };
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
