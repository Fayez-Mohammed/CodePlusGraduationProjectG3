using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.SectionDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface ISectionService
    {
        Task<Result<IReadOnlyList<SectionDTO>>> GetAllAsync(string courseId);
        Task<Result<SectionResponseDTO>> CreateAsync(CreateSectionDTO sectionDTO);
        Task<Result<SectionResponseDTO>> UpdateAsync(UpdateSectionDTO sectionDTO);
        Task<Result> RemoveAsync(string id);
        Task<Result<List<ReOrderDTO>>> ReOrderSectionsAsync(List<ReOrderDTO> reOrderDTOs);
    }
}
