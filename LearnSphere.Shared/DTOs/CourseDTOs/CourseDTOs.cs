using LearnSphere.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class CreateCourseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public CourseLevel Level { get; set; }
        public string CategoryId { get; set; }
    }
    public class TogglePublishResultDTO
    {
        public string Id { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
   

}
