using EcommerceBackend.Application.DTOs;
using EcommerceBackend.Application.Models;

namespace EcommerceBackend.Application.Abstractions;

/// <summary>Read-side store: returns projections straight to DTOs, never tracked entities.</summary>
public interface IProductReadRepository
{
    Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<ProductResponse>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
}
