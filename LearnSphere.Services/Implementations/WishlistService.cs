using LearnSphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<WishlistItemDto>> GetWishlistAsync(string studentId, CancellationToken ct = default)
        {
            return await _context.Set<Wishlist>()
                .AsNoTracking()
                .Where(w => w.StudentId == studentId && w.Course.IsPublished)
                .OrderByDescending(w => w.AddedAt)
                .Select(w => new WishlistItemDto
                {
                    CourseId = w.CourseId,
                    Title = w.Course.Title,
                    Slug = w.Course.Slug,
                    ThumbnailUrl = w.Course.ThumbnailUrl,
                    Price = w.Course.Price,
                    Level = w.Course.Level.ToString(),
                    AddedAt = w.AddedAt
                })
                .ToListAsync(ct);
        }

        public async Task<WishlistResult> AddAsync(string studentId, string courseId, CancellationToken ct = default)
        {
            var courseExists = await _context.Set<Course>()
                .AnyAsync(c => c.Id == courseId && c.IsPublished, ct);
            if (!courseExists) return WishlistResult.CourseNotFound;

            var exists = await _context.Set<Wishlist>()
                .AnyAsync(w => w.StudentId == studentId && w.CourseId == courseId, ct);
            if (exists) return WishlistResult.AlreadyExists;

            _context.Set<Wishlist>().Add(new Wishlist
            {
                StudentId = studentId,
                CourseId = courseId
            });

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                return WishlistResult.AlreadyExists;
            }

            return WishlistResult.Success;
        }

        public async Task<WishlistResult> RemoveAsync(string studentId, string courseId, CancellationToken ct = default)
        {
            var item = await _context.Set<Wishlist>()
                .FirstOrDefaultAsync(w => w.StudentId == studentId && w.CourseId == courseId, ct);

            if (item is null) return WishlistResult.NotInWishlist;

            _context.Set<Wishlist>().Remove(item);
            await _context.SaveChangesAsync(ct);
            return WishlistResult.Success;
        }
    }
}
