using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.Profile
{
    public class UploadImageDTO
    {
        public IFormFile File { get; set; } = default!;
    }
}
