using LearnSphere.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.SectionDTOs
{
    public class SectionDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int DisplayOrder { get; set; }
        public int LessonsCount { get; set; }
        public int TotalDurationInMinutes { get; set; }
        public bool HasQuiz { get; set; }

        public IReadOnlyList<SectionLessonsDTO> Lessons { get; set; }
    }
    public class SectionLessonsDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public LessonType Type { get; set; }
        public int DurationMinutes {  get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFreePreview { get; set; }
    }
    public class CreateSectionDTO
    {
        public required string CourseId { get; set; }
        public required string Title { get; set; }
    }
    public class UpdateSectionDTO
    {
        public required string SectionId { get; set; }
        public required string Title { get; set; }
    }
    public class SectionResponseDTO
    {
        public string Id { get; set;}
        public  string CourseId { get; set; }
        public  string Title { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ReOrderDTO
    {
        public string Id { get; set;}
        public int DisplayOrder { get; set; }
    }

}
