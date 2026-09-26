using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.DAL.Models.SystemModels;

public class Certificate :BaseEntity
{
    public string CertificateUrl { get; set; } = null!;

    // Foreign Keys
    public string StudentId { get; set; } = null!;
    public string CourseId { get; set; } = null!;

    // Navigation Properties
    public virtual ApplicationUser Student { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}
