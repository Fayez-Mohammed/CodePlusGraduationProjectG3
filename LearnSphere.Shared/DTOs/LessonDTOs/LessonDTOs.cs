using LearnSphere.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.LessonDTOs
{
    public class LessonDTOs
    {

    }
    /*  "title": "Welcome to the Course — Updated",
  "durationMinutes": 6,
  "isFreePreview": true,
  "textContent": null*/
    public class LessonReOrderDTO
    {
        public string Id { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class UpdateLessonDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsFreePreview { get; set; }
        public string? TextContent { get; set; }
    }
    public class UploadLessonDTO
    {
        public string Id { get; set; }
        public int DurationInMinutes { get; set; }
        public IFormFile File { get; set; }
    }
    public class UploadLessonResponseDTO
    {
        public string Id { get; set; }
        public string ContentURL { get; set; }
        public int DurationInMinutes { get; set; }
        public bool HasContent { get; set; }
    }
    public class LessonDetailsDTO
    {
        public string Id { get; set; }
        public string SectionId { get; set; }
        public string CourseId { get; set; }
        public string Title { get; set; }
        public LessonType Type { get; set; }
        public string? ContentURL { get; set; }
        public string? TextContent { get; set; }
        public int DurationInMinutes { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFreePreview { get; set; }
        public ProgressDTO? Progress { get; set; }
        public LessonNavigationDTO? Navigation { get; set; }
       
    }
    public class ProgressDTO
    {
        public bool IsCompleted { get; set; }
        public int LastWatchedSeconds { get; set; }
    }
    public class LessonNavigationDTO
    {
        public string? PreviewsLessonId { get; set; }
        public string? NextLessonId { get; set; }
    }
    public class LessonsDTO
    {
        public string Id { get; set; }
        public string SectionId { get; set; }
        public string Title { get; set; }
        public LessonType Type { get; set; }
        public string? ContentURL { get; set; }
        public string? TextContent { get; set; }
        public int DurationInMinutes { get; set; }
        public  int DisplayOrder { get; set; }
        public bool IsFreePreview { get; set; }
        public bool IsCompleted { get; set; }
        public int LastWatchedSeconds { get; set; }


    }
    public class CreateLessonDTO
    {
        public string SectionId { get; set; }
        public string Title { get; set; }
        public LessonType Type { get; set; }

        public string? ContentUrl { get; set; }
        public string? TextContent { get; set; }
        public int DurationMinutes { get; set; } = 0;
        public bool IsFreePreview { get; set; }

    }
    public class CreateLessonResponseDTO
    {
        public string Id { get; set; }
        public string SectionId { get; set; }
        public string Title { get; set; }
        public LessonType Type { get; set; }
        public bool IsFreePreview { get; set; }
        public int DisplayOrder { get; set; }
        public bool HasContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
