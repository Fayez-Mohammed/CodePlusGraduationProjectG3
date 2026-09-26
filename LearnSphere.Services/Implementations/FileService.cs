using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        private static readonly HashSet<string> AllowedImageExtensions =

        new(StringComparer.OrdinalIgnoreCase)
        {
          
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };
        private static readonly HashSet<string> AllowedVideoExtensions =

        new(StringComparer.OrdinalIgnoreCase)
        {
             ".mp4", ".mkv", ".webm"
        };
        private const long MaxImageSize = 5 * 1024 * 1024;
        private const long MaxVideoSize = 1000 * 1024 * 1024;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<Result<string>> UploadImageAsync(IFormFile file,string folderName)
        {
            return await UploadFileAsync(file, AllowedImageExtensions, MaxImageSize,folderName);
           


        }

        public async Task<Result<string>> UploadVideoAsync(IFormFile file,string folderName)
        {
            return await UploadFileAsync(file, AllowedVideoExtensions, MaxVideoSize, folderName);

        }
        private async Task<Result<string>> UploadFileAsync(IFormFile file,HashSet<string> allowedExtensions,long maxSize,string folderName)
        {
            if (file == null || file.Length == 0)
                return Result<string>.Failure("Upload your File first");

            if (file.Length > maxSize)
                return Result<string>.Failure($"File size can't exceed {maxSize/(1024*1024)} MB");


            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))

                return Result<string>.Failure("Not allowed extension");
            var fileName = $"{Guid.NewGuid()}{ext}";

            var uploadFolder = Path.Combine(_env.WebRootPath,"uploads", folderName);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var physicalPath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imagePathForDB = $"uploads/{folderName}/{fileName}";

            return Result<string>.Success(imagePathForDB);

        }

        public Result RemoveFile(string relativeFilePath)
        {
            if (relativeFilePath is null)
                return Result.Failure("relative file path is empty");
            var normalizedFilePath = relativeFilePath
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar)
                .TrimStart(Path.DirectorySeparatorChar);
            var phisicalPath=Path.Combine(_env.WebRootPath,normalizedFilePath);
            var fullWebRootPath = Path.GetFullPath(_env.WebRootPath);
            var fullPhisicalPath = Path.GetFullPath(phisicalPath);
            if (!fullPhisicalPath.StartsWith(fullWebRootPath, StringComparison.OrdinalIgnoreCase))
                return Result.Failure("Invalid file path");
            if(!File.Exists(phisicalPath))
                return Result.Failure("file not exist on phisical disc");
            File.Delete(phisicalPath);
            return Result.Success();

        }
    }

}
