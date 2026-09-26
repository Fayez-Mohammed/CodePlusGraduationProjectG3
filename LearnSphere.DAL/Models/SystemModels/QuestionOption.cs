using LearnSphere.DAL.Models.BaseModels;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class QuestionOption : BaseEntity
    {
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; } = false;

        // Foreign Key
        public string QuestionId { get; set; } = null!;

        // Navigation Property
        public virtual Question Question { get; set; } = null!;
    }
}