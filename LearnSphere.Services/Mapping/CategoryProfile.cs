using AutoMapper;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Shared.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Mapping
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryDTO, Category>();
          //  CreateMap<UpdateCategoryDTO, Category>();
        }
    }
}
