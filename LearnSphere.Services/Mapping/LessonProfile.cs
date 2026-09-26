using AutoMapper;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Shared.DTOs.LessonDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Mapping
{
    public class LessonProfile:Profile
    {
        public LessonProfile()
        {
            CreateMap<CreateLessonDTO, Lesson>();
            CreateMap<Lesson, CreateLessonResponseDTO>()
                .ForMember(x=>x.HasContent,op=>op.MapFrom(src=>!string.IsNullOrWhiteSpace(src.ContentUrl)|| !string.IsNullOrWhiteSpace(src.TextContent)));
            CreateMap<UpdateLessonDTO,Lesson>().ReverseMap();
        }
    }
}
