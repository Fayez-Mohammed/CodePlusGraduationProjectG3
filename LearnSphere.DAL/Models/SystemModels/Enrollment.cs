using LearnSphere.DAL.Models.BaseModels;
using System;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class Enrollment : BaseEntity
    {
        public decimal CompletionPercentage { get; set; } = 0.00m;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }

        public string StudentId { get; set; } = null!;
        public string CourseId { get; set; }

        public virtual ApplicationUser Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
    }
}