using AutoMapper;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Repo.Specifications;
using LearnSphere.Services.Helpers;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.LessonDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class LessonService : ILessonsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFileService _fileService;
        public LessonService(IMapper mapper,IUnitOfWork unit,IHttpContextAccessor contextAccessor,IFileService fileService)
        {
            _mapper = mapper;
            _unit = unit;
            _contextAccessor = contextAccessor;
            _fileService = fileService;
            
        }
        public async Task<Result<CreateLessonResponseDTO>> CreateLessonsAsync(CreateLessonDTO lessonDTO)
        {
            if (lessonDTO == null)
                return Result<CreateLessonResponseDTO>.Failure("Please Fill The Form");
            var exist = await _unit.Repository<Section>().AnyAsync(x => x.Id == lessonDTO.SectionId);
            if(!exist)
                return Result<CreateLessonResponseDTO>.Failure("Section Not Found");

            var spec = new BaseSpecification<Lesson>(s=>s.SectionId==lessonDTO.SectionId);
            var lesson=_mapper.Map<Lesson>(lessonDTO);
            var lastOrder =await _unit.Repository<Lesson>().MaxAsync(spec,x=>(int?)x.DisplayOrder)??0;

            lesson.DisplayOrder = lastOrder + 1;
            await _unit.Repository<Lesson>().AddAsync(lesson);
            await _unit.CompleteAsync();
            var response = _mapper.Map<CreateLessonResponseDTO>(lesson);
            return Result<CreateLessonResponseDTO>.Success(response);
            
            
        }

        public async Task<Result> DeleteAsync(string id)
        {
            if (id is null)
                return Result.Failure("id is required");
            var lesson = await _unit.Repository<Lesson>().GetByIdAsync(id);
            if(lesson is null)
                return Result.Failure("lesson not found");
            if(lesson.ContentUrl is null)
                return Result.Failure("there is no files to remove");
            var result = _fileService.RemoveFile(lesson.ContentUrl);
            if (result.IsSuccess)
                return Result.Success();
            return Result.Failure(result.Error);


        }

        public async Task<Result<IReadOnlyList<LessonsDTO>>> GetALlAsync(string sectionId)
        {
            if (sectionId == null)
                return Result<IReadOnlyList<LessonsDTO>>.Failure("sectionId is required");
            var exist = await _unit.Repository<Section>().AnyAsync(s => s.Id == sectionId);
            if(!exist) return Result<IReadOnlyList<LessonsDTO>>.Failure("section is not found",404);
            var currentUserId = UserHelper.GetCurrentUserId(_contextAccessor);
            var spec =new BaseSpecification<Lesson>(l=>l.SectionId==sectionId);
            spec.ApplyNoTracking();
            spec.AddOrderBy(l => l.DisplayOrder);
            var lessons = await _unit.Repository<Lesson>().SelectAsync(spec,
                l => new LessonsDTO
                {
                    Id = l.Id,
                    SectionId = l.SectionId,
                    Title = l.Title,
                    Type = l.Type,
                    ContentURL = l.ContentUrl,
                    TextContent = l.TextContent,
                    DisplayOrder = l.DisplayOrder,
                    DurationInMinutes = l.DurationMinutes,
                    LastWatchedSeconds = !string.IsNullOrWhiteSpace(currentUserId) ? l.LessonProgresses.Where(lp => lp.StudentId == currentUserId)
                    .Select(lp => (int?)lp.LastWatchedSeconds).FirstOrDefault() ?? 0:0,
                    IsFreePreview = l.IsFreePreview,
                    IsCompleted = !string.IsNullOrWhiteSpace(currentUserId) &&l.LessonProgresses.Any(lp=>lp.StudentId == currentUserId&&lp.IsCompleted)
                }
                );
            return Result<IReadOnlyList<LessonsDTO>>.Success(lessons);
        }

        public async Task<Result<LessonDetailsDTO>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result<LessonDetailsDTO>.Failure("id is required");
            var currentUserId = UserHelper.GetCurrentUserId(_contextAccessor);
            var spec = new BaseSpecification<Lesson>(l => l.Id == id);
            
            spec.ApplyNoTracking();
            var lesson = await _unit.Repository<Lesson>().GetEntityWithSpecificationAsync(spec,
                l=>new LessonDetailsDTO
                {
                    Id = l.Id,
                    CourseId = l.Section.CourseId,
                    SectionId = l.SectionId,
                    ContentURL = l.ContentUrl,
                    TextContent = l.TextContent,
                    Title = l.Title,
                    Type = l.Type,
                    DisplayOrder = l.DisplayOrder,
                    DurationInMinutes = l.DurationMinutes,
                    IsFreePreview = l.IsFreePreview,
                    Progress = !string.IsNullOrWhiteSpace(currentUserId) ? l.LessonProgresses.Where(lp => lp.StudentId == currentUserId).Select(lp => new ProgressDTO
                    {
                        IsCompleted = lp.IsCompleted,
                        LastWatchedSeconds = lp.LastWatchedSeconds,
                    }).FirstOrDefault() : null,
                    Navigation=new LessonNavigationDTO
                    {
                        PreviewsLessonId=l.Section.Lessons.Where(pr=>pr.DisplayOrder<l.DisplayOrder).OrderByDescending(pr=>pr.DisplayOrder).Select(x=>x.Id).FirstOrDefault(),
                        NextLessonId=l.Section.Lessons.Where(nx=>nx.DisplayOrder>l.DisplayOrder).OrderBy(pr=>pr.DisplayOrder).Select(x=>x.Id).FirstOrDefault(),
                    }
                }
                );
            if(lesson==null)
                return Result<LessonDetailsDTO>.Failure("Lesson Not Found",404);

            return Result<LessonDetailsDTO>.Success(lesson);
        }

        public async Task<Result<List<LessonReOrderDTO>>> ReOrderLessosAsync(List<LessonReOrderDTO> lessonsDTO)
        {
            if (!lessonsDTO.Any())
                return Result<List<LessonReOrderDTO>>.Failure("please order them first");
            var dic = lessonsDTO.ToDictionary(x => x.Id, x => x.DisplayOrder);
            var ids=dic.Keys.ToList();
            var spec = new BaseSpecification<Lesson>(x => ids.Contains(x.Id));
            var lessons = await _unit.Repository<Lesson>().SelectAsync(spec);
            var updateLessons = lessons.ToList();
            foreach(var lesson in lessons)
            {
                if (lesson==null) continue;
                if(dic.TryGetValue(lesson.Id, out var lessonOrder))
                    lesson.DisplayOrder=lessonOrder;
            }
            await _unit.Repository<Lesson>().UpdateRangeAsync(updateLessons);
            await _unit.CompleteAsync();
            var response = lessons.Select(x => new LessonReOrderDTO
            {
                Id = x.Id,
                DisplayOrder = x.DisplayOrder,
            }).ToList();
            return Result<List<LessonReOrderDTO>>.Success(response);
        }

        public async Task<Result<UpdateLessonDTO>> UpdateLessonAsync(UpdateLessonDTO lessonDTO)
        {
            if (lessonDTO == null)
                return Result<UpdateLessonDTO>.Failure("please fill the form");
            if (string.IsNullOrWhiteSpace(lessonDTO.Id))
                return Result<UpdateLessonDTO>.Failure("id is required");
            var lesson=await _unit.Repository<Lesson>().GetByIdAsync(lessonDTO.Id);
            if(lesson==null)
                return Result<UpdateLessonDTO>.Failure("Lesson not found",404);

            _mapper.Map(lessonDTO, lesson);
            await _unit.Repository<Lesson>().UpdateAsync(lesson);
            await _unit.CompleteAsync();
         

            return Result<UpdateLessonDTO>.Success(lessonDTO);
        }

        public async Task<Result<UploadLessonResponseDTO>> UploadAsync(UploadLessonDTO lessonDTO)
        {
            if (lessonDTO == null)
                return Result<UploadLessonResponseDTO>.Failure("Please fill the form");
            var lesson = await _unit.Repository<Lesson>().GetByIdAsync(lessonDTO.Id);
            if(lesson == null)
                return Result<UploadLessonResponseDTO>.Failure("Lesson Not Found",404);
            var lessonURLForDB = await _fileService.UploadVideoAsync(lessonDTO.File, "Lessons");
           if(!lessonURLForDB.IsSuccess)
                return Result<UploadLessonResponseDTO>.Failure("error while Uploading the lesson");
            lesson.ContentUrl = lessonURLForDB.Value;
            lesson.DurationMinutes = lessonDTO.DurationInMinutes;
            await _unit.Repository<Lesson>().UpdateAsync(lesson);
            await _unit.CompleteAsync();
            var response = new UploadLessonResponseDTO
            {
                Id = lesson.Id,
                ContentURL = lesson.ContentUrl,
                DurationInMinutes = lesson.DurationMinutes,
                HasContent = !string.IsNullOrWhiteSpace(lesson.ContentUrl)

            };
            return Result<UploadLessonResponseDTO>.Success(response);



        }
    }
}
