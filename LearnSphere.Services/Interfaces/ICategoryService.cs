using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<IReadOnlyList<CategoryDTO>>> GetAllAsync();
        Task<Result<CategoryDTO>> GetById(string Id);
       Task<Result<string>> AddAsync(CreateCategoryDTO categoryDTO);
        Task<Result> UpdateAsync (UpdateCategoryDTO categoryDTO);
        Task<Result> RemoveAsync (string id);

    }
}
