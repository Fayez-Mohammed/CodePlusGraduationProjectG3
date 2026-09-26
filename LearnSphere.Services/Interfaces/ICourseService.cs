using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Result<CreateCourseResponseDTO>> CreateAsync(CreateCourseDTO courseDTO);
        Task<Result<UpdateCourseDTO>> UpdateAsync(UpdateCourseDTO courseDTO);
        Task<Result<PagedResult<CoursesDTO>>> GetAllAsync(string search,int pageNumber,int pageSize);
        Task<Result<CourseDetailsDTO>> GetByIdAsync(string id);
        Task<Result> DeleteAsync(string id);
        Task<Result<UploadThumbnailResultDTO>> UploadThumbnailAsync(UploadThumbnailDTO thumbnailDTO);
        Task<Result<UploadPreviewVideoResultDTO>> UploadPreviewVideoAsync(UploadPreviewVideoDTO previewVideoDTO);
        Task<Result<TogglePublishResultDTO>> TogglePublishAsync(string courseId);
    }
}
