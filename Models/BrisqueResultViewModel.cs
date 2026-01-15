namespace Brisqueometer.Models;

public class BrisqueResultViewModel
{
    public double Score { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string QualityDescription { get; set; } = string.Empty;
}
