using LearnSphere.DAL.Models.BaseModels;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class QuizAttempt : BaseEntity
    {
        public decimal ScoreAchieved { get; set; }
        public bool IsPassed { get; set; }

        // Foreign Keys
        public string StudentId { get; set; } = null!;
        public string QuizId { get; set; } = null!;

        // Navigation Properties
        public virtual ApplicationUser Student { get; set; } = null!;
        public virtual Quiz Quiz { get; set; } = null!;
    }
}
