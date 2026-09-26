using LearnSphere.DAL.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class Course : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? PreviewVideoUrl { get; set; }
        public decimal Price { get; set; }
        public CourseLevel Level { get; set; } // Enum: Beginner, Intermediate, Advanced
        public bool IsPublished { get; set; } = false;

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Section> Sections { get; set; }= new List<Section>();
        public virtual ICollection<Enrollment> Enrollments { get; set; }= new List<Enrollment>();
        public virtual ICollection<Certificate> Certificates { get; set; }= new List<Certificate>();
        public virtual ICollection<Review> Reviews { get; set; }= new List<Review>();
        public virtual ICollection<Wishlist> Wishlists { get; set; }= new List<Wishlist>();
        public virtual ICollection<Payment> Payments { get; set; }= new List<Payment>();
    }
    }
