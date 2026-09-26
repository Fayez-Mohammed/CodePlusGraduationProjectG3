using LearnSphere.Services.Mapping;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.Profile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface IProfileService
    {
        Task<Result<string>> UploadProfilePictureAsync(IFormFile file,string userId);
        Task<Result<GetProfileDTO>> UpdateProfileAsync(UpdateProfileDTO profileDTO);
        Task<Result<GetProfileDTO>> GetProfileAsync(string userId);
    }
}
