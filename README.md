# Brisqueometer

A .NET 8.0 MVC web application for assessing image quality using the BRISQUE (Blind/Referenceless Image Spatial Quality Evaluator) algorithm.

## Features

- Upload images in JPG, JPEG, PNG, or BMP format
- Get perceptual quality scores without requiring a reference image
- User-friendly web interface with Bootstrap styling
- Supports files up to 10MB

## Requirements

- .NET 8.0 SDK
- BRISQUE model files (see setup instructions below)

## Setup Instructions

### 1. Install .NET 8.0 SDK

If you don't have .NET 8.0 SDK installed, download it from:
https://dotnet.microsoft.com/download/dotnet/8.0

### 2. Download BRISQUE Model Files

The application requires two model files to function. Download them from the OpenCV repository:

1. **brisque_model_live.yml**
   - URL: https://github.com/opencv/opencv_contrib/raw/master/modules/quality/samples/brisque_model_live.yml

2. **brisque_range_live.yml**
   - URL: https://github.com/opencv/opencv_contrib/raw/master/modules/quality/samples/brisque_range_live.yml

Place both files in the `Models` directory at the root of the project.

### 3. Restore NuGet Packages

```bash
dotnet restore
```

This will install the Luxoria.Algorithm.BrisqueScore package (version 3.0.3.4100) and its dependencies.

### 4. Build the Project

```bash
dotnet build
```

### 5. Run the Application

```bash
dotnet run
```

The application will start and be available at:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

## Usage

1. Navigate to the home page
2. Click "Choose File" and select an image
3. Click "Analyze Image Quality"
4. View the BRISQUE score and quality assessment

### Understanding BRISQUE Scores

BRISQUE scores typically range from 0 to 100:
- **0-20**: Excellent quality
- **20-40**: Good quality
- **40-60**: Fair quality
- **60-80**: Poor quality
- **80-100**: Very poor quality

Lower scores indicate better image quality.

## Project Structure

```
Brisqueometer/
├── Controllers/
│   └── HomeController.cs         # Handles image upload and processing
├── Models/
│   ├── ImageUploadViewModel.cs   # View model for file upload
│   ├── BrisqueResultViewModel.cs # View model for results
│   ├── brisque_model_live.yml    # BRISQUE model file (required)
│   └── brisque_range_live.yml    # BRISQUE range file (required)
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml          # Upload page
│   │   └── Result.cshtml         # Results page
│   └── Shared/
│       ├── _Layout.cshtml        # Main layout
│       └── Error.cshtml          # Error page
├── wwwroot/
│   ├── css/
│   │   └── site.css              # Custom styles
│   ├── js/
│   │   └── site.js               # JavaScript
│   └── uploads/                  # Uploaded images (created at runtime)
├── appsettings.json              # Application configuration
├── Program.cs                    # Application entry point
└── Brisqueometer.csproj          # Project file
```

## Configuration

Edit `appsettings.json` to customize settings:

```json
{
  "BrisqueSettings": {
    "ModelPath": "Models/brisque_model_live.yml",
    "RangePath": "Models/brisque_range_live.yml",
    "MaxFileSizeBytes": 10485760
  }
}
```

## Technologies Used

- **ASP.NET Core 8.0 MVC**: Web framework
- **Luxoria.Algorithm.BrisqueScore**: BRISQUE implementation
- **Bootstrap 5**: UI framework
- **jQuery**: JavaScript library

## License

This project uses the Luxoria.Algorithm.BrisqueScore package, which is licensed under Apache 2.0.

## Troubleshooting

### Error: Model files not found

Ensure you've downloaded the BRISQUE model files and placed them in the `Models` directory.

### Error: Unable to load native library

The Luxoria.Algorithm.BrisqueScore package includes native libraries for Windows (x86, x64, arm64). Ensure you're running on a supported platform.

## About BRISQUE

BRISQUE (Blind/Referenceless Image Spatial Quality Evaluator) is a no-reference image quality assessment algorithm. It uses natural scene statistics to quantify image quality without requiring a pristine reference image, making it ideal for real-world applications where reference images are unavailable.

## References

- **Luxoria.Algorithm.BrisqueScore NuGet Package**: https://www.nuget.org/packages/Luxoria.Algorithm.BrisqueScore/
- **Source Code**: https://github.com/LuxoriaSoft/brisque_impl
- **OpenCV BRISQUE Implementation**: https://github.com/opencv/opencv_contrib/tree/master/modules/quality
