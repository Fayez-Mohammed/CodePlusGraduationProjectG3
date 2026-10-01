using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CourseDTOs;
using LearnSphere.Shared.DTOs.LessonDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnSphere.API.Controllers
{
    [ApiController]
    [Route("progress")]
    [Authorize]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }


        // PUT /progress/lessons/{lessonId}
        [HttpPut("lessons/{lessonId}")]
        public async Task<ActionResult<Result<UpdateLessonProgressDTO>>>
            UpdateLessonProgress(
                string lessonId,
                [FromBody] UpdateLessonProgressDTO dto,
                CancellationToken ct)
        {
            var result = await _progressService
                .UpdateLessonAsync(
                    lessonId,
                    dto,
                    ct);

            return StatusCode(
                result.StatusCode,
                result);
        }


        // POST /progress/lessons/{lessonId}/complete
        [HttpPost("lessons/{lessonId}/complete")]
        public async Task<ActionResult<Result>>
            CompleteLesson(
                string lessonId,
                CancellationToken ct)
        {
            var result = await _progressService
                .CompleteLessonAsync(
                    lessonId,
                    ct);

            return StatusCode(
                result.StatusCode,
                result);
        }


        // GET /progress/courses/{courseId}
        [HttpGet("courses/{courseId}")]
        public async Task<ActionResult<Result<CourseProgressDTO>>>
            GetCourseProgress(
                string courseId,
                CancellationToken ct)
        {
            var result = await _progressService
                .GetCourseProgressAsync(
                    courseId,
                    ct);

            return StatusCode(
                result.StatusCode,
                result);
        }
    }
}