using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Repo.Specifications;
using LearnSphere.Services.Helpers;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.EnrollmentDTOs;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace LearnSphere.Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unit;
        private readonly IHttpContextAccessor _contextAccessor;

        public EnrollmentService(IUnitOfWork unit, IHttpContextAccessor contextAccessor)
        {
            _unit = unit;
            _contextAccessor = contextAccessor;
        }

        private static readonly Expression<Func<Enrollment, EnrollmentDTO>> EnrollmentSelector = e => new EnrollmentDTO
        {
            Id = e.Id,
            CourseId = e.CourseId,
            CourseTitle = e.Course.Title,
            CourseSlug = e.Course.Slug,
            ThumbnailUrl = e.Course.ThumbnailUrl,
            Level = e.Course.Level.ToString(),
            CompletionPercentage = e.CompletionPercentage,
            IsCompleted = e.IsCompleted,
            CompletedAt = e.CompletedAt,
            EnrolledAt = e.CreatedAt
        };

        private static readonly Expression<Func<Enrollment, AdminEnrollmentDTO>> AdminEnrollmentSelector = e => new AdminEnrollmentDTO
        {
            Id = e.Id,
            StudentId = e.StudentId,
            StudentName = e.Student.FullName,
            StudentEmail = e.Student.Email!,
            CourseId = e.CourseId,
            CourseTitle = e.Course.Title,
            CompletionPercentage = e.CompletionPercentage,
            IsCompleted = e.IsCompleted,
            CompletedAt = e.CompletedAt,
            EnrolledAt = e.CreatedAt
        };

        public async Task<Result<EnrollmentDTO>> Enroll(EnrollRequestDTO request)
        {
            var studentId = UserHelper.GetCurrentUserId(_contextAccessor);
            if (string.IsNullOrWhiteSpace(studentId))
                return Result<EnrollmentDTO>.Failure("User not found", 401);

            if (request == null || string.IsNullOrWhiteSpace(request.CourseId))
                return Result<EnrollmentDTO>.Failure("Course Id is required");

            var course = await _unit.Repository<Course>().GetByIdAsync(request.CourseId, true);
            if (course == null || !course.IsPublished)
                return Result<EnrollmentDTO>.Failure("Course Not Found", 404);

            bool alreadyEnrolled = await _unit.Repository<Enrollment>()
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == request.CourseId);
            if (alreadyEnrolled)
                return Result<EnrollmentDTO>.Failure("You are already enrolled in this course", 409);

            //if (course.Price > 0)
            //{
            //    bool hasPaid = await _unit.Repository<Payment>().AnyAsync(p =>
            //        p.StudentId == studentId &&
            //        p.CourseId == request.CourseId &&
            //        p.Status == PaymentStatus.Success);
            //    if (!hasPaid)
            //        return Result<EnrollmentDTO>.Failure("Payment is required to enroll in this course", 402);
            //}

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = request.CourseId
            };

            await _unit.Repository<Enrollment>().AddAsync(enrollment);
            await _unit.CompleteAsync();

            var dto = new EnrollmentDTO
            {
                Id = enrollment.Id,
                CourseId = course.Id,
                CourseTitle = course.Title,
                CourseSlug = course.Slug,
                ThumbnailUrl = course.ThumbnailUrl,
                Level = course.Level.ToString(),
                CompletionPercentage = enrollment.CompletionPercentage,
                IsCompleted = enrollment.IsCompleted,
                CompletedAt = enrollment.CompletedAt,
                EnrolledAt = enrollment.CreatedAt
            };
            return Result<EnrollmentDTO>.Success(dto);
        }

        public async Task<Result<PagedResult<EnrollmentDTO>>> GetMyCourses(int pageNumber, int pageSize)
        {
            var studentId = UserHelper.GetCurrentUserId(_contextAccessor);
            if (string.IsNullOrWhiteSpace(studentId))
                return Result<PagedResult<EnrollmentDTO>>.Failure("User not found", 401);

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int skip = (pageNumber - 1) * pageSize;

            var spec = new BaseSpecification<Enrollment>(e => e.StudentId == studentId);
            var totalCount = await _unit.Repository<Enrollment>().CountAsync(spec);

            spec.ApplyPagination(skip, pageSize);
            spec.ApplyNoTracking();
            spec.AddOrderByDesc(e => e.CreatedAt);

            var items = await _unit.Repository<Enrollment>().SelectAsync(spec, EnrollmentSelector);

            var pagedResult = new PagedResult<EnrollmentDTO>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
            return Result<PagedResult<EnrollmentDTO>>.Success(pagedResult);
        }

        public async Task<Result<EnrollmentDTO>> GetById(string id)
        {
            var studentId = UserHelper.GetCurrentUserId(_contextAccessor);
            if (string.IsNullOrWhiteSpace(studentId))
                return Result<EnrollmentDTO>.Failure("User not found", 401);

            if (string.IsNullOrWhiteSpace(id))
                return Result<EnrollmentDTO>.Failure("Id is required");

            var spec = new BaseSpecification<Enrollment>(e => e.Id == id && e.StudentId == studentId);
            spec.ApplyNoTracking();

            var enrollment = await _unit.Repository<Enrollment>()
                .GetEntityWithSpecificationAsync(spec, EnrollmentSelector);
            if (enrollment == null)
                return Result<EnrollmentDTO>.Failure("Enrollment Not Found", 404);

            return Result<EnrollmentDTO>.Success(enrollment);
        }

        public async Task<Result<PagedResult<AdminEnrollmentDTO>>> GetAll(string? search, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int skip = (pageNumber - 1) * pageSize;

            var spec = new BaseSpecification<Enrollment>(e =>
                string.IsNullOrWhiteSpace(search)
                || e.Student.FullName.Contains(search.Trim())
                || e.Student.Email!.Contains(search.Trim())
                || e.Course.Title.Contains(search.Trim()));
            var totalCount = await _unit.Repository<Enrollment>().CountAsync(spec);

            spec.ApplyPagination(skip, pageSize);
            spec.ApplyNoTracking();
            spec.AddOrderByDesc(e => e.CreatedAt);

            var items = await _unit.Repository<Enrollment>().SelectAsync(spec, AdminEnrollmentSelector);

            var pagedResult = new PagedResult<AdminEnrollmentDTO>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
            return Result<PagedResult<AdminEnrollmentDTO>>.Success(pagedResult);
        }
    }
}