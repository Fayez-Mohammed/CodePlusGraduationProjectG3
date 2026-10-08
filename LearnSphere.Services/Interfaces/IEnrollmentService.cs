using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.EnrollmentDTOs;

namespace LearnSphere.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Result<EnrollmentDTO>> Enroll(EnrollRequestDTO request);
        Task<Result<PagedResult<EnrollmentDTO>>> GetMyCourses(int pageNumber, int pageSize);
        Task<Result<EnrollmentDTO>> GetById(string id);
        Task<Result<PagedResult<AdminEnrollmentDTO>>> GetAll(string? search, int pageNumber, int pageSize);
    }
}