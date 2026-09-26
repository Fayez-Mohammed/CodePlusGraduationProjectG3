using Microsoft.AspNetCore.Http;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class UploadThumbnailDTO
    {
        public required string CourseId { get; set; }
        public required IFormFile ThumbnaiImage { get; set; }
    }

}
