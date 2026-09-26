using Grpc.Core;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Services.Implementations;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CategoryDTOs;
using LearnSphere.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Instructor")]
    public class CategoriesController : ControllerBase
    {
        ICategoryService service;
        public CategoriesController(ICategoryService categoryService)
        {
            this.service = categoryService;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result=await service.GetAllAsync();
          
            return Ok(result.Value);

        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The requested category.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var result=await service.GetById(id);
           
            return Ok(result.Value);

        }


        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="categoryDTO">The category data to add.</param>
        /// <returns>The created category.</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateCategoryDTO categoryDTO)
        {
            var res = await service.AddAsync(categoryDTO);
            return Created("", res);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryDTO">The category data to update.</param>
        /// <returns>The updated category.</returns>
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm]UpdateCategoryDTO categoryDTO)
        {
            var res =await service.UpdateAsync(categoryDTO);
           
            return Ok(res);
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var res=await service.RemoveAsync(id);
        
            return Ok(res);
        }
    }
}
