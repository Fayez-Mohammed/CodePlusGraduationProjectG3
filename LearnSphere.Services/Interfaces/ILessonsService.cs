using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.LessonDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface ILessonsService
    {
        Task<Result<CreateLessonResponseDTO>> CreateLessonsAsync(CreateLessonDTO lessonDTO);
        Task<Result<IReadOnlyList<LessonsDTO>>> GetALlAsync(string sectionId);
        Task<Result<LessonDetailsDTO>> GetByIdAsync(string id);
        Task<Result<UploadLessonResponseDTO>> UploadAsync(UploadLessonDTO lessonDTO);
        Task<Result<UpdateLessonDTO>> UpdateLessonAsync(UpdateLessonDTO lessonDTO);
        Task<Result> DeleteAsync(string id);
        Task<Result<List<LessonReOrderDTO>>> ReOrderLessosAsync(List<LessonReOrderDTO> lessonsDTO);

    }
}
