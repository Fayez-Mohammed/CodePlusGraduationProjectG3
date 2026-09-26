namespace LearnSphere.Shared.DTOs.CourseDTOs
{
    public class SectionDetailsDTO
    {
        public string Id {  set; get; }
        public string Title { get; set; }
        public int DisplayOrder { get; set; }
        public List<LessonDetailsDTO> Lessons { get; set; }

    }

}
