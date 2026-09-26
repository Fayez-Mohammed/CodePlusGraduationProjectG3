using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.AuthDTOs;
using LearnSphere.Shared.DTOs.StudentDTOs;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user,string password);
        Task<bool> CheckPasswordAsync(ApplicationUser user,string password);
        Task UpdateProfilePictureAsync(string userId,string imagePath);
    }
}
