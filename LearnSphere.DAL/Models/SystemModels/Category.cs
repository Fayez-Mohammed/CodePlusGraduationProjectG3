using LearnSphere.DAL.Models.BaseModels;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class Category :BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
