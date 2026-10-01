using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class CourseProgressDTO
    {
        public double CompletionPercentage { get; set; }
        public string? LastWatchedLessonId { get; set; }
        public string? LastWatchedLessonTitle { get; set; }
        public int TotalTimeMinutes { get; set; }
    }
}
