using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs.SectionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Instructor")]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionService _service;
        public SectionsController(ISectionService service)
        {
            _service = service;

        }
        /// <summary>
        /// Retrieves all sections for a specific course by its ID.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet("{courseId}/sections")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string courseId)
        {
            var result = await _service.GetAllAsync(courseId);
            return Ok(result);
        }
        /// <summary>
        /// Creates a new section for a course.
        /// </summary>
        /// <param name="sectionDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody]CreateSectionDTO sectionDTO)
        {
            var result = await _service.CreateAsync(sectionDTO);
            return Created("", result);
        }
        /// <summary>
        /// Updates an existing section.
        /// </summary>
        /// <param name="sectionDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateSection([FromBody] UpdateSectionDTO sectionDTO)
        {
            var result = await _service.UpdateAsync(sectionDTO);
            return Ok(result);
        }
        /// <summary>
        /// Deletes a section by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]

        public async Task<IActionResult> RemoveSection(string id)
        {
            var result = await _service.RemoveAsync(id);
            return Ok(result);
        }
        /// <summary>
        /// Reorders sections based on the provided list of ReOrderDTOs.
        /// </summary>
        /// <param name="reOrderDTOs"></param>
        /// <returns></returns>
        [HttpPatch("reorder")]
        public async Task<IActionResult> ReOrderSections([FromBody] List<ReOrderDTO> reOrderDTOs)
        {
            var result=await _service.ReOrderSectionsAsync(reOrderDTOs);
            return Ok(result);
        }

    }
}
