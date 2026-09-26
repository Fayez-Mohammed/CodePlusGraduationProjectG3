using LearnSphere.DAL.Models;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class UpdateCourseDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public CourseLevel Level { get; set; }
        public string CategoryId { get; set; }

    }

}
