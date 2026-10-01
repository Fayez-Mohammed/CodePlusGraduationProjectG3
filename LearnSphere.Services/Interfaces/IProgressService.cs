using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CourseDTOs;
using LearnSphere.Shared.DTOs.LessonDTOs;

namespace LearnSphere.Services.Interfaces
{
    public interface IProgressService
    {
        Task<Result<UpdateLessonProgressDTO>> UpdateLessonAsync(
            string lessonId,
            UpdateLessonProgressDTO updateLessonProgressDTO,
            CancellationToken ct = default);

        Task<Result> CompleteLessonAsync(
            string lessonId,
            CancellationToken ct = default);

        Task<Result<CourseProgressDTO>> GetCourseProgressAsync(
            string courseId,
            CancellationToken ct = default);
    }
}