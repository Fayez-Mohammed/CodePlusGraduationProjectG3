using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.DAL.Models.BaseModels
{
    public class BaseEntity:IAuditableEntity,ISoftDelete
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public BaseEntity()
        {
            Id=Guid.NewGuid().ToString();
        }
    }
}
