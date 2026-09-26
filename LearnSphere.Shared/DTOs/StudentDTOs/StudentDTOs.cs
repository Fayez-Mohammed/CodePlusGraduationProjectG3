using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.StudentDTOs
{
    public class StudentDetailsDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ImagePath { get; set; }
        public string? Bio { get; set; }
        public DateTime DateOfCreation { get; set; }
        public StudentStatus Status { get; set; }
    }
    public class StudentStatus
    {
        public int EnrolledCoursesCount { get; set; }
        public int CompletedCoursesCount { get; set; }
        public int CertificatesEarned { get; set; }
        public decimal TotalSpent { get; set; }
        public int WishListCount { get; set; }
        public int ReviewWritten {  get; set; } 
    }
    public class StudentsDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ImagePath { get; set; }
        public int EnrolledCoursesCount { get; set; }
        public decimal? TotalSpent { get; set; }
        public DateTime DateOfCreation { get; set; }


    }
}
