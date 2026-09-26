using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.DAL.Models.BaseModels
{
    public class ApplicationUser:IdentityUser,IAuditableEntity
    {
        public string FullName { get ; set; }
        public UserTypes UserType { get; set; } = UserTypes.Student;
        public string? ImagePath { get; set; }
        public string? Bio {  get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
       
        // Navigation Properties for Student Activities
        public virtual  ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        //public virtual  ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public virtual  ICollection<Payment> Payments { get; set; } = new List<Payment>();
       // public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }


}

