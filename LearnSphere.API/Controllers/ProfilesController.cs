using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _service;
        public ProfilesController(IProfileService service)
        {
            _service = service;
        }

        /// <summary>
        /// Uploads a profile picture.
        /// </summary>
        /// <param name="image">The image to upload.</param>
        /// <returns>The result of the upload operation.</returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDTO image)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=await _service.UploadProfilePictureAsync(image.File, userId);
            return Ok(result);
        }

        /// <summary>
        /// Updates the profile of the currently authenticated user.
        /// </summary>
        /// <param name="profileDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO profileDTO)
        {
           // profileDTO.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resut=await _service.UpdateProfileAsync(profileDTO);
            return Ok(resut);
        }
        /// <summary>
        /// Retrieves the profile of the currently authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId=User.FindFirstValue(claimType:ClaimTypes.NameIdentifier);
            var result=await _service.GetProfileAsync(userId);
            return Ok(result);
        }
    }
}

