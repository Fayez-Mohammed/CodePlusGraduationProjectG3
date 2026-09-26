using LearnSphere.DAL.Models.BaseModels;
using System.Collections.Generic;

namespace LearnSphere.DAL.Models.SystemModels
{
    public class Section : BaseEntity
    {
        public string Title { get; set; } = null!;
        public int DisplayOrder { get; set; }

        public string CourseId { get; set; } = null!;

        public virtual Quiz? Quiz { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual Course? Course { get; set; }
    }
}