using System;
using System.Collections.Generic;
using LearnSphere.shared.Enums;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<IReadOnlyList<WishlistItemDto>> GetWishlistAsync(string studentId, CancellationToken ct = default);
        Task<WishlistResult> AddAsync(string studentId, string courseId, CancellationToken ct = default);
        Task<WishlistResult> RemoveAsync(string studentId, string courseId, CancellationToken ct = default);
    }
}
