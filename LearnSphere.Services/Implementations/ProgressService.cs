using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Repo.Specifications;
using LearnSphere.Services.Helpers;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CourseDTOs;
using LearnSphere.Shared.DTOs.LessonDTOs;
using Microsoft.AspNetCore.Http;

namespace LearnSphere.Services.Implementations
{
    public class ProgressService : IProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProgressService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // =========================================================
        // PUT /progress/lessons/{lessonId}
        // =========================================================

        public async Task<Result<UpdateLessonProgressDTO>> UpdateLessonAsync(
            string lessonId,
            UpdateLessonProgressDTO updateLessonProgressDTO,
            CancellationToken ct = default)
        {
            // Get current logged-in user
            var studentId =
                UserHelper.GetCurrentUserId(_httpContextAccessor);

            if (studentId == null)
            {
                return Result<UpdateLessonProgressDTO>.Failure(
                    "User not found");
            }

            // Check that lesson exists
            var lesson = await _unitOfWork
                .Repository<Lesson>()
                .GetByIdAsync(lessonId);

            if (lesson == null)
            {
                return Result<UpdateLessonProgressDTO>.Failure(
                    "Lesson not found");
            }

            // Find progress for this student + this lesson
            var spec = new BaseSpecification<LessonProgress>(
                x => x.StudentId == studentId &&
                     x.LessonId == lessonId
            );

            var progress = await _unitOfWork
                .Repository<LessonProgress>()
                .GetEntityWithSpecificationAsync(spec);

            // Progress doesn't exist -> create it
            if (progress == null)
            {
                progress = new LessonProgress
                {
                    StudentId = studentId,
                    LessonId = lessonId,
                    LastWatchedSeconds =
                        updateLessonProgressDTO.LastWatchedSeconds,
                    IsCompleted = false
                };

                await _unitOfWork
                    .Repository<LessonProgress>()
                    .AddAsync(progress);
            }
            else
            {
                // Progress exists -> update watched position
                progress.LastWatchedSeconds =
                    updateLessonProgressDTO.LastWatchedSeconds;

                await _unitOfWork
                    .Repository<LessonProgress>()
                    .UpdateAsync(progress);
            }

            // Save changes
            await _unitOfWork.CompleteAsync();

            return Result<UpdateLessonProgressDTO>.Success(
                updateLessonProgressDTO);
        }


        // =========================================================
        // POST /progress/lessons/{lessonId}/complete
        // =========================================================

        public async Task<Result> CompleteLessonAsync(
            string lessonId,
            CancellationToken ct = default)
        {
            // Get current logged-in user
            var studentId =
                UserHelper.GetCurrentUserId(_httpContextAccessor);

            if (studentId == null)
            {
                return Result.Failure("User not found");
            }

            // Check that lesson exists
            var lesson = await _unitOfWork
                .Repository<Lesson>()
                .GetByIdAsync(lessonId);

            if (lesson == null)
            {
                return Result.Failure("Lesson not found");
            }

            // Find progress for this student + this lesson
            var spec = new BaseSpecification<LessonProgress>(
                x => x.StudentId == studentId &&
                     x.LessonId == lessonId
            );

            var progress = await _unitOfWork
                .Repository<LessonProgress>()
                .GetEntityWithSpecificationAsync(spec);

            // Progress doesn't exist -> create completed progress
            if (progress == null)
            {
                progress = new LessonProgress
                {
                    StudentId = studentId,
                    LessonId = lessonId,
                    LastWatchedSeconds = 0,
                    IsCompleted = true
                };

                await _unitOfWork
                    .Repository<LessonProgress>()
                    .AddAsync(progress);
            }
            else
            {
                // Progress exists -> mark as completed
                progress.IsCompleted = true;

                await _unitOfWork
                    .Repository<LessonProgress>()
                    .UpdateAsync(progress);
            }

            // Save changes
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }


        // =========================================================
        // GET /progress/courses/{courseId}
        // =========================================================

        public async Task<Result<CourseProgressDTO>> GetCourseProgressAsync(
            string courseId,
            CancellationToken ct = default)
        {
            // Get current logged-in user
            var studentId =
                UserHelper.GetCurrentUserId(_httpContextAccessor);

            if (studentId == null)
            {
                return Result<CourseProgressDTO>.Failure(
                    "User not found");
            }

            // Check that course exists
            var course = await _unitOfWork
                .Repository<Course>()
                .GetByIdAsync(courseId);

            if (course == null)
            {
                return Result<CourseProgressDTO>.Failure(
                    "Course not found");
            }

            // Get all progress records of this student
            // for lessons that belong to this course
            var spec = new BaseSpecification<LessonProgress>(
                x => x.StudentId == studentId &&
                     x.Lesson.Section.CourseId == courseId
            );

            // We need Lesson and Section information
            spec.AddInclude(x => x.Lesson);
            spec.AddInclude(x => x.Lesson.Section);

            var progressList = await _unitOfWork
                .Repository<LessonProgress>()
                .SelectAsync(spec);

            // Get all lessons in this course
            var lessonsSpec = new BaseSpecification<Lesson>(
                x => x.Section.CourseId == courseId
            );

            var lessons = await _unitOfWork
                .Repository<Lesson>()
                .SelectAsync(lessonsSpec);

            // Avoid division by zero
            if (lessons.Count == 0)
            {
                return Result<CourseProgressDTO>.Success(
                    new CourseProgressDTO
                    {
                        CompletionPercentage = 0,
                        LastWatchedLessonId = null,
                        LastWatchedLessonTitle = null,
                        TotalTimeMinutes = 0
                    });
            }

            // Count completed lessons
            var completedLessonsCount =
                progressList.Count(x => x.IsCompleted);

            // Calculate completion percentage
            var completionPercentage =
                (double)completedLessonsCount /
                lessons.Count *
                100;

            // Find the most recently updated progress
            var lastWatchedProgress = progressList
                .OrderByDescending(x =>
                    x.UpdatedAt ?? x.CreatedAt)
                .FirstOrDefault();

            // Calculate total watched time
            var totalWatchedSeconds =
                progressList.Sum(x => x.LastWatchedSeconds);

            var totalTimeMinutes =
                totalWatchedSeconds / 60;

            var result = new CourseProgressDTO
            {
                CompletionPercentage = completionPercentage,

                LastWatchedLessonId =
                    lastWatchedProgress?.LessonId,

                LastWatchedLessonTitle =
                    lastWatchedProgress?.Lesson?.Title,

                TotalTimeMinutes =
                    totalTimeMinutes
            };

            return Result<CourseProgressDTO>.Success(result);
        }
    }
}