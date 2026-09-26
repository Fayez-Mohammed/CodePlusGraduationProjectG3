using LearnSphere.DAL.Models;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class LessonDetailsDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public LessonType Type { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsFreePreview { get; set; }
        public string? ContetURL { get; set; }
    }

}
