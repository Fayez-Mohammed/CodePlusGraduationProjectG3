using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.DAL.Models.SystemModels;

public class Wishlist:BaseEntity
{
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Composite Foreign Keys
    public string StudentId { get; set; } = null!;
    public string CourseId { get; set; } = null!;

    // Navigation Properties
    public virtual ApplicationUser Student { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}