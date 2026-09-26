using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {
        public string? Id {  get; set; }
        public string ImageURL { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateCategoryDTO
    {
        public string Id {  get; set; }
        public IFormFile? File { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
    }
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? File { get; set; }
    }
}
