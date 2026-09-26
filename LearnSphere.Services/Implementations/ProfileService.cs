using AutoMapper;
using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Services.Helpers;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.Profile;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IFileService _imageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IHttpContextAccessor _ContextAccessor;
        public ProfileService(IFileService imageService,
            IUserService userService,IMapper mapper,
            IUnitOfWork unit,IHttpContextAccessor contextAccessor)
        {
            _imageService = imageService;
            _userService = userService;
            _mapper = mapper;
            _unit = unit;
            _ContextAccessor = contextAccessor;
        }
        public async Task<Result<string>> UploadProfilePictureAsync(IFormFile file,string userId)
        {
            var result= await _imageService.UploadImageAsync(file,"ProfilePictures");
            if(!result.IsSuccess)
                return result;
            var imagePath = result.Value;
            
            await _userService.UpdateProfilePictureAsync(userId, imagePath);
            return Result<string>.Success(imagePath);

           
        }

        public async Task<Result<GetProfileDTO>> UpdateProfileAsync(UpdateProfileDTO profileDTO)
        {
            if(profileDTO ==null)
                return Result<GetProfileDTO>.Failure("Please fill the form");
            var currentUserId =UserHelper.GetCurrentUserId(_ContextAccessor);
            if (currentUserId == null)
                return Result<GetProfileDTO>.Failure("User Not Found");
            var user=await _userService.GetByIdAsync(currentUserId);
            if (user == null)
                throw new NotFoundException("User Not Found");
            _mapper.Map(profileDTO, user);
           await _unit.CompleteAsync();
            var userDTO = _mapper.Map<GetProfileDTO>(user);
            return Result<GetProfileDTO>.Success(userDTO);
           

        }

        public async Task<Result<GetProfileDTO>> GetProfileAsync(string userId)
        {
            if (userId == null)
                return Result<GetProfileDTO>.Failure("User Not Found");
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User Not Found");
            var userDTO = _mapper.Map<GetProfileDTO>(user);
            return Result<GetProfileDTO>.Success(userDTO);

        }
    }
}
