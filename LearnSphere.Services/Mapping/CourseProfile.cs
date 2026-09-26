using AutoMapper;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Shared.DTOs.CategoryDTOs;
using LearnSphere.Shared.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Mapping
{
    public class CourseProfile:Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<Course,CreateCourseResponseDTO>();
            CreateMap<UpdateCourseDTO,Course>();
        }
    }
}
