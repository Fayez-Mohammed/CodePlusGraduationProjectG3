using LearnSphere.DAL.Models;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class CourseDetailsDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public string? ThumbnailURL { get; set; }
        public string? PreviewVideoURL { get; set; }
        public CourseLevel Level { get; set; }
        public string? Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsInWishlist { get; set; }
        public bool HasQuiz { get; set; }
        public CategoryLookupDto Category { get; set; }
        public CourseStatsDto Status { get; set; }
        public List<SectionDetailsDTO> Sections { get; set; }
       
    }

}
