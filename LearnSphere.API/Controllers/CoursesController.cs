using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace LearnSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Instructor")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;
        public CoursesController(ICourseService service)
        {
            _service = service;
        }


        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="courseDTO">The course data to create.</param>
        /// <returns>The created course.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO courseDTO)
        {
            var result = await _service.CreateAsync(courseDTO);
            return Created("", result);
        }


        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="courseDTO">The course data to update.</param>
        /// <returns>The updated course.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseDTO courseDTO)
        {
            var result = await _service.UpdateAsync(courseDTO);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves all courses.
        /// </summary>
        /// <param name="search">The search term.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A list of courses.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllAsync(search, pageNumber, pageSize);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>The requested course.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }


        /// <summary>
        /// Deletes a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var result = await _service.DeleteAsync(id);
            return NoContent();
        }


        /// <summary>
        /// Uploads a thumbnail for a course.
        /// </summary>
        /// <param name="thumbnailDTO">The thumbnail data to upload.</param>
        /// <returns>The result of the upload operation.</returns>
        [HttpPatch("thumbnail")]
        public async Task<IActionResult> UploadThumbnail([FromForm] UploadThumbnailDTO thumbnailDTO)
        {
            var result =await _service.UploadThumbnailAsync(thumbnailDTO);
            return Ok(result);
        }


        /// <summary>
        /// Uploads a preview video for a course.
        /// </summary>
        /// <param name="previewVideoDTO">The preview video data to upload.</param>
        /// <returns>The result of the upload operation.</returns>
        [HttpPatch("previewVideo")]
        public async Task<IActionResult> UploadPreviewVideo([FromForm] UploadPreviewVideoDTO previewVideoDTO)
        {
            var result =await _service.UploadPreviewVideoAsync(previewVideoDTO);
            return Ok(result);
        }


        /// <summary>
        /// Toggles the publish status of a course.
        /// </summary>
        /// <param name="id">The ID of the course to toggle.</param>
        /// <returns>The result of the toggle operation.</returns>
        [HttpPatch("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublish(string id)
        {
            var result =await _service.TogglePublishAsync(id);
            return Ok(result);
        }
    }
}
