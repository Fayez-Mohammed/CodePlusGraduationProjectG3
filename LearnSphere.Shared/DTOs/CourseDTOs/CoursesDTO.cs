using LearnSphere.DAL.Models;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class CoursesDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public string? ThumbnailURL { get; set; }
        public CourseLevel Level { get; set; }
        public CategoryLookupDto Category { get; set; }
       public CourseStatsDto Status { get; set; }

    }

}
