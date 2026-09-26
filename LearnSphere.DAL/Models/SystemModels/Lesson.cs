using LearnSphere.DAL.Models.BaseModels;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class Lesson : BaseEntity
    {
        public string Title { get; set; } = null!;
        public LessonType Type { get; set; } // Enum: Video, Text, PDF, File
        public string? ContentUrl { get; set; }
        public string? TextContent { get; set; }
        public int DurationMinutes { get; set; } = 0;
        public int DisplayOrder { get; set; }
        public bool IsFreePreview { get; set; } = false;

        // Foreign Key
        public string? SectionId { get; set; }

        // Navigation Properties
        public virtual Section Section { get; set; } = null!;
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    }
}