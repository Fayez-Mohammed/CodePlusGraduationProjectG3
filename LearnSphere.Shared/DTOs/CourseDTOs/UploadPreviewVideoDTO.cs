using Microsoft.AspNetCore.Http;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class UploadPreviewVideoDTO
    {
        public required string CourseId { get; set; }
        public required IFormFile PreviewVideo { get; set; }
    }

}
