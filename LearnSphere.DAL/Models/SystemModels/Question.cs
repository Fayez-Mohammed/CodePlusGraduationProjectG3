using LearnSphere.DAL.Models.BaseModels;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class Question : BaseEntity
    {
        public string QuestionText { get; set; } = null!;
        public int Points { get; set; } = 1;

        public string QuizId { get; set; } = null!;

        public virtual Quiz Quiz { get; set; } = null!;
        public virtual ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    }
}