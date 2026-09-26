using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Result<PagedResult<StudentsDTO>>> GetStudents(string search,int pageNumber,int pageSize);
        Task<Result<StudentDetailsDTO>> GetById(string id);
    }
}
