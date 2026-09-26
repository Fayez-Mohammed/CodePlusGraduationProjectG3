namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class CreateCourseResponseDTO
    {
        public string Id { get; set; }
        public string Slug {  get; set; }
        public string Title { get; set; }
        public bool IsPublished { get; set; }
        public string? ThumnailURL { get; set; }
        public string? PreviewVideoURL { get; set; }
        public DateTime CreatedAt { get; set; }

    }

}
