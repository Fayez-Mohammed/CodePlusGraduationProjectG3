using LearnSphere.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface IFileService
    {
        Task<Result<string>> UploadImageAsync(IFormFile file,string folderName);
        Task<Result<string>> UploadVideoAsync(IFormFile file,string folderName);
        Result RemoveFile(string relativeFilePath);
    }
}
