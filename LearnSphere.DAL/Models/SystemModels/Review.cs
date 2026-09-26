using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.DAL.Models.SystemModels;

public class Review : BaseEntity
{
    public int Rating { get; set; } // 1 to 5
    public string? Comment { get; set; }

    // Foreign Keys
    public string StudentId { get; set; } = null!;
    public string CourseId { get; set; } = null!;

    // Navigation Properties
    public virtual ApplicationUser Student { get; set; } = null!;
    public virtual  Course Course { get; set; } = null!;
}