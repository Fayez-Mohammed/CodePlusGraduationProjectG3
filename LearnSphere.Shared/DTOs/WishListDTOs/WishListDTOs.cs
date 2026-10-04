using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.WishListDTOs
{
    public class WishlistItemDto
    {
        public string CourseId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public string Level { get; set; } = null!;
        public DateTime AddedAt { get; set; }
    }
}
