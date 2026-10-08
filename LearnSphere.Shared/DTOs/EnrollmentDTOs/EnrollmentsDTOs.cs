using System;
using System.ComponentModel.DataAnnotations;

namespace LearnSphere.Shared.DTOs.EnrollmentDTOs
{
    public class EnrollRequestDTO
    {
        [Required]
        public string CourseId { get; set; } = null!;
        public string? CouponCode { get; set; }
    }

    public class EnrollmentDTO
    {
        public string Id { get; set; } = null!;
        public string CourseId { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string CourseSlug { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public string Level { get; set; } = null!;
        public decimal CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime EnrolledAt { get; set; }
    }

    public class AdminEnrollmentDTO
    {
        public string Id { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string CourseId { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public decimal CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}