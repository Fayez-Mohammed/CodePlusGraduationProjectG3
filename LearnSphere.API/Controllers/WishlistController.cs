using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnSphere.API.Controllers
{
    [ApiController]
    [Route("wishlist")]
    [Authorize(Roles = "Student")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService) => _wishlistService = wishlistService;

        private string StudentId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<WishlistItemDto>>> Get(CancellationToken ct)
        {
            var items = await _wishlistService.GetWishlistAsync(StudentId, ct);
            return Ok(items);
        }

        [HttpPost("{courseId}")]
        public async Task<IActionResult> Add(string courseId, CancellationToken ct)
        {
            var result = await _wishlistService.AddAsync(StudentId, courseId, ct);

            return result switch
            {
                WishlistResult.Success => StatusCode(StatusCodes.Status201Created),
                WishlistResult.CourseNotFound => NotFound(new { message = "Course not found." }),
                WishlistResult.AlreadyExists => Conflict(new { message = "Course is already in your wishlist." }),
                _ => BadRequest()
            };
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> Remove(string courseId, CancellationToken ct)
        {
            var result = await _wishlistService.RemoveAsync(StudentId, courseId, ct);

            return result switch
            {
                WishlistResult.Success => NoContent(),
                WishlistResult.NotInWishlist => NotFound(new { message = "Course is not in your wishlist." }),
                _ => BadRequest()
            };
        }
    }
}
