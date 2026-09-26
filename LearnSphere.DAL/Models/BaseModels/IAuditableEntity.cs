using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.DAL.Models.BaseModels
{
    public interface IAuditableEntity
    {
        public DateTime? UpdatedAt { get; set; }
    }
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
