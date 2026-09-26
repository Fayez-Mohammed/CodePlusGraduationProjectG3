using AutoMapper;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Repo.Specifications;
using LearnSphere.Services.Helpers;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace LearnSphere.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _contextAccessor;
        public CourseService(IUnitOfWork unit,IMapper mapper,IFileService fileService,IHttpContextAccessor contextAccessor)
        {
            _unit = unit;
            _mapper = mapper;
            _fileService = fileService;
            _contextAccessor = contextAccessor;
        }
       
        public async Task<Result<CreateCourseResponseDTO>> CreateAsync(CreateCourseDTO courseDTO)
        {
            if (courseDTO == null)
                return Result<CreateCourseResponseDTO>.Failure("Please fill the form");
            bool exist = await _unit.Repository<Category>().AnyAsync(c => c.Id == courseDTO.CategoryId);
            if (!exist)
                return Result<CreateCourseResponseDTO>.Failure("Category not found");

            var course = _mapper.Map<Course>(courseDTO);
            var slug = CourseHelper.GenerateSlug(courseDTO.Title);

            bool slugExist = await _unit.Repository<Course>().AnyAsync(c => c.Slug == slug);
            course.Slug = slugExist ? $"{slug}-{Guid.NewGuid().ToString()[..4]}" : slug;

          
            await _unit.Repository<Course>().AddAsync(course);

            await _unit.CompleteAsync();
            var courseResponse=_mapper.Map<CreateCourseResponseDTO>(course);
            return Result<CreateCourseResponseDTO>.Success(courseResponse);
            

            
        }

        public async Task<Result> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result.Failure("Id is required");
            var course=await _unit.Repository<Course>().GetByIdAsync(id);
            if (course == null)
                return Result.Failure("Course Not Found");
            course.Title =  $"{course.Title}__Deleted";
            course.Slug =  $"{course.Slug}__Deleted";
           
            await _unit.Repository<Course>().RemoveAsync(course);
            await _unit.CompleteAsync();
            return Result.Success();
        }

        public async Task<Result<PagedResult<CoursesDTO>>> GetAllAsync(string? search,int pageNumber,int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize=pageSize < 1 ? 10 : pageSize;
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
       
            var filterSpec = new BaseSpecification<Course>(c=>string.IsNullOrWhiteSpace(search)||c.Title.Contains(search.Trim()));
            var totalCount=await _unit.Repository<Course>().CountAsync(filterSpec);
                
             filterSpec.ApplyPagination(skip, take);
            filterSpec.ApplyNoTracking();
            filterSpec.AddOrderByDesc(c => c.CreatedAt);          
            var courses = await _unit.Repository<Course>().SelectAsync(filterSpec, c => new CoursesDTO
            {
                Id = c.Id,
                Title = c.Title,
                Slug = c.Slug,
                Level = c.Level,
                Price = c.Price,
                ThumbnailURL=c.ThumbnailUrl,
                Category=new CategoryLookupDto { Id=c.CategoryId,Name=c.Category.Name},
                Status=new CourseStatsDto {
                    AvgRating = c.Reviews.Select(r=>(double?)r.Rating).Average()??0.0,
                    ReviewCount = c.Reviews.Count,
                    EnrolledStudentsCount =c.Enrollments.Count,
                    LessonsCount = c.Sections.SelectMany(l => l.Lessons).Count(),
                    TotalDurationMinutes = c.Sections.SelectMany(l => l.Lessons).Sum(m => m.DurationMinutes) }
                

            });
            var pagedResult = new PagedResult<CoursesDTO>
            {
                Items = courses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
            return Result<PagedResult<CoursesDTO>>.Success(pagedResult);
        }

        public async Task<Result<CourseDetailsDTO>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result<CourseDetailsDTO>.Failure("id is required");
            var currentUserId = UserHelper.GetCurrentUserId(_contextAccessor);
            var spec = new BaseSpecification<Course>(c => c.Id == id);
            spec.ApplyNoTracking();
            var course = await _unit.Repository<Course>().GetEntityWithSpecificationAsync(spec, c => new CourseDetailsDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ThumbnailURL = c.ThumbnailUrl,
                PreviewVideoURL = c.PreviewVideoUrl,
                Price = c.Price,
                Slug = c.Slug,
                CreatedAt = c.CreatedAt,
                Level = c.Level,
                IsPublished = c.IsPublished,
                HasQuiz = c.Sections.Any(q => q.Quiz != null),
                IsInWishlist = !string.IsNullOrEmpty(currentUserId) && c.Wishlists.Any(u=>u.StudentId== currentUserId),
                IsEnrolled = !string.IsNullOrEmpty(currentUserId) && c.Enrollments.Any(u => u.StudentId == currentUserId),
                Status = new CourseStatsDto
                {
                    AvgRating = c.Reviews.Select(r => (double?)r.Rating).Average() ?? 0.0,
                    ReviewCount = c.Reviews.Count,
                    EnrolledStudentsCount = c.Enrollments.Count,
                    LessonsCount = c.Sections.SelectMany(l => l.Lessons).Count(),
                    TotalDurationMinutes = c.Sections.SelectMany(l => l.Lessons).Sum(m => m.DurationMinutes)

                },
                Category = new CategoryLookupDto { Id = c.CategoryId, Name = c.Category.Name },
                Sections = c.Sections.OrderBy(s=>s.DisplayOrder).Select(s => new SectionDetailsDTO
                {
                    Id = s.Id,
                    DisplayOrder = s.DisplayOrder,
                    Title = s.Title,
                    Lessons = s.Lessons.OrderBy(l => l.DisplayOrder).Select(l => new LessonDetailsDTO
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Type = l.Type,
                        ContetURL = l.ContentUrl,
                        DurationMinutes = l.DurationMinutes,
                        IsFreePreview = l.IsFreePreview,
                    }).ToList()
                }).ToList()

            });
            if (course == null)
                return Result<CourseDetailsDTO>.Failure("Course Not Found",404);
            return Result<CourseDetailsDTO>.Success(course);
        }

        public async Task<Result<UpdateCourseDTO>> UpdateAsync(UpdateCourseDTO courseDTO)
        {
            if (courseDTO == null)
                return Result<UpdateCourseDTO>.Failure("Please fill the form");
            bool exist = await _unit.Repository<Category>().AnyAsync(c => c.Id == courseDTO.CategoryId);
            if (!exist)
                return Result<UpdateCourseDTO>.Failure("Category not found");
            var course=await _unit.Repository<Course>().GetByIdAsync(courseDTO.Id);
            if(course==null)
                return Result<UpdateCourseDTO>.Failure("Course not found");

            _mapper.Map(courseDTO,course);
            var slug = CourseHelper.GenerateSlug(courseDTO.Title);


            bool slugExist = await _unit.Repository<Course>().AnyAsync(c => c.Slug == slug && c.Id != course.Id);
            course.Slug = slugExist ? $"{slug}-{Guid.NewGuid().ToString()[..4]}" : slug;
            await _unit.CompleteAsync();
            return Result<UpdateCourseDTO>.Success(courseDTO);
           
        }

        public async Task<Result<UploadThumbnailResultDTO>> UploadThumbnailAsync(UploadThumbnailDTO thumbnailDTO)
        {
            var course = await _unit.Repository<Course>().GetByIdAsync(thumbnailDTO.CourseId);
            if (course == null)
                return Result<UploadThumbnailResultDTO>.Failure("Course Not Found");

            var imagePathInDb = await _fileService.UploadImageAsync(thumbnailDTO.ThumbnaiImage, "ThumbnailImages");
            if (imagePathInDb == null||!imagePathInDb.IsSuccess)
                return Result<UploadThumbnailResultDTO>.Failure("Uploading Image Failure");
            course.ThumbnailUrl = imagePathInDb.Value;
            await _unit.CompleteAsync();
            var result = new UploadThumbnailResultDTO
            {
                Id = course.Id,
                ThumbnaiURL = course.ThumbnailUrl
            };
            return Result<UploadThumbnailResultDTO>.Success(result);
        }
   
        public async Task<Result<UploadPreviewVideoResultDTO>> UploadPreviewVideoAsync(UploadPreviewVideoDTO previewVideoDTO)
        {
            var course = await _unit.Repository<Course>().GetByIdAsync(previewVideoDTO.CourseId);
            if (course == null)
                return Result<UploadPreviewVideoResultDTO>.Failure("Course Not Found");

            var PreviewVideoUrlInDb = await _fileService.UploadVideoAsync(previewVideoDTO.PreviewVideo, "PreviewVideos");
            if (PreviewVideoUrlInDb == null||!PreviewVideoUrlInDb.IsSuccess)
                return Result<UploadPreviewVideoResultDTO>.Failure("Uploading Preview Video Failure");
            course.PreviewVideoUrl = PreviewVideoUrlInDb.Value;
            await _unit.CompleteAsync();
            var result = new UploadPreviewVideoResultDTO
            {
                Id = course.Id,
                PreviewVideoURL = course.PreviewVideoUrl
            };
            return Result<UploadPreviewVideoResultDTO>.Success(result);
        }

        public async Task<Result<TogglePublishResultDTO>> TogglePublishAsync(string courseId)
        {
            if (courseId == null)
                return Result<TogglePublishResultDTO>.Failure("Course Id is Required");
            var course = await _unit.Repository<Course>().GetByIdAsync(courseId);
            if(course == null)
                return Result<TogglePublishResultDTO>.Failure("Course not found");
            course.IsPublished = !course.IsPublished;
            await _unit.CompleteAsync();
            var result = new TogglePublishResultDTO
            {
                Id = course.Id,
                IsPublished = course.IsPublished,
                UpdatedAt = course.UpdatedAt
            };
            return Result<TogglePublishResultDTO>.Success(result);


        }
    }
}
