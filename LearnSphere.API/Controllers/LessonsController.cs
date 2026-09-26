using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.LessonDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Instructor")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonsService _service;
        public LessonsController(ILessonsService service)
        {
            _service = service;
        }


        /// <summary>
        /// Creates a new lesson.
        /// </summary>
        /// <param name="lessonDTO">The lesson data to create.</param>
        /// <returns>The created lesson.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody]CreateLessonDTO lessonDTO)
        {
            var result=await _service.CreateLessonsAsync(lessonDTO);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves all lessons for a given section.
        /// </summary>
        /// <param name="sectionId">The ID of the section to retrieve lessons for.</param>
        /// <returns>A list of lessons.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessons([FromQuery]string sectionId)
        {
            var result=await _service.GetALlAsync(sectionId);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves a lesson by its ID.
        /// </summary>
        /// <param name="id">The ID of the lesson to retrieve.</param>
        /// <returns>The requested lesson.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var result=await _service.GetByIdAsync(id);
            return Ok(result);
        }


        /// <summary>
        /// Uploads a lesson.
        /// </summary>
        /// <param name="lessonDTO">The lesson data to upload.</param>
        /// <returns>The result of the upload operation.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadLesson([FromForm]UploadLessonDTO lessonDTO)
        {
            var result=await _service.UploadAsync(lessonDTO);
            return Ok(result);
        }


        /// <summary>
        /// Updates an existing lesson.
        /// </summary>
        /// <param name="lessonDTO">The lesson data to update.</param>
        /// <returns>The updated lesson.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateLesson([FromBody]UpdateLessonDTO lessonDTO)
        {
            var result =await _service.UpdateLessonAsync(lessonDTO);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a lesson by its ID.
        /// </summary>
        /// <param name="id">The ID of the lesson to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(string id)
        {
            var result=await _service.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Reorders the lessons.
        /// </summary>
        /// <param name="lessonsDTO">The list of lessons with their new order.</param>
        /// <returns>The result of the reordering operation.</returns>
        [HttpPatch("reorder")]
        public async Task<IActionResult> ReOrderLessons([FromBody] List<LessonReOrderDTO> lessonsDTO)
        {
            var result=await _service.ReOrderLessosAsync(lessonsDTO);
            return Ok(result);
        }
    }
}
