using LearnSphere.DAL.Models.BaseModels;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class LessonProgress : BaseEntity
    {
        public bool IsCompleted { get; set; } = false;
        public int LastWatchedSeconds { get; set; } = 0;

        // Foreign Keys
        public string StudentId { get; set; } = null!;
        public string LessonId { get; set; } = null!;

        // Navigation Properties
        public virtual ApplicationUser Student { get; set; } = null!;
        public virtual Lesson Lesson { get; set; } = null!;
    }
}