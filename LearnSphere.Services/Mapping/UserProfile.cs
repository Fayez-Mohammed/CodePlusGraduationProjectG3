using AutoMapper;
using FluentValidation;
using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.Shared.DTOs.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UpdateProfileDTO, ApplicationUser>();
            CreateMap<ApplicationUser, GetProfileDTO>();
        }
    }
}
