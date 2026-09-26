using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.Repo.Implementations;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using LearnSphere.Shared.Enums;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.StudentDTOs;
using Microsoft.EntityFrameworkCore;

namespace LearnSphere.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<ApplicationUser> GetByEmailAsync(string email)
        =>await _userManager.FindByEmailAsync(email);
        public async Task<ApplicationUser> GetByIdAsync(string email)
        =>await _userManager.FindByIdAsync(email);

       



        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user,string password)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(password);
            var result=await  _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(";", result.Errors.Select(e => e.Description)));
           result= await _userManager.AddToRoleAsync(user, user.UserType.ToString());
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(";", result.Errors.Select(e => e.Description)));

            return user;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            bool Iscorrect=await _userManager.CheckPasswordAsync(user, password);
            return Iscorrect;
        }

        public async Task UpdateProfilePictureAsync(string userId, string imagePath)
        {
            var user =await _userManager.FindByIdAsync(userId);
            user.ImagePath = imagePath;
          await  _unitOfWork.CompleteAsync();
           
        }
    }
}
