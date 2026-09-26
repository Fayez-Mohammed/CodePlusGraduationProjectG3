using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnSphere.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Instructor")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        public StudentsController(IStudentService service)
        {
            _service=service;
        }
        /// <summary>
        /// Get all students with optional search and pagination
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? search,[FromQuery]int pageNumber=1,[FromQuery]int pageSize=10)
        {
            var result=await _service.GetStudents(search,pageNumber,pageSize);
            return (Ok(result));
        }
        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result=await _service.GetById(id);
            return Ok(result);
        }
    }
}
