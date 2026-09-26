using LearnSphere.DAL.Models.BaseModels;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class Quiz : BaseEntity
    {
        public string Title { get; set; } = null!;
        public decimal PassingScore { get; set; } = 50.00m;

        public string SectionId { get; set; } = null!;

        public virtual Section Section { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}