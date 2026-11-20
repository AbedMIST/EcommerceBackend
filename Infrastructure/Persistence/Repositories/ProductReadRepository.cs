using EcommerceBackend.Application.DTOs;
using EcommerceBackend.Application.Models;
using EcommerceBackend.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Infrastructure.Persistence.Repositories;

internal sealed class ProductReadRepository(AppDbContext db) : IProductReadRepository
{
    public Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(p.Id, p.Name, p.Description, p.Sku, p.Price, p.StockQuantity, p.CreatedAt, p.UpdatedAt))
            .FirstOrDefaultAsync(ct);

    public async Task<PagedResult<ProductResponse>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = db.Products.AsNoTracking();

        if (!string.IsNullOrEmpty(search))
        {
            var pattern = $"%{search}%";
            query = query.Where(p => EF.Functions.Like(p.Name, pattern) || EF.Functions.Like(p.Sku, pattern));
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.CreatedAt).ThenBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponse(p.Id, p.Name, p.Description, p.Sku, p.Price, p.StockQuantity, p.CreatedAt, p.UpdatedAt))
            .ToListAsync(ct);

        return new PagedResult<ProductResponse>(items, page, pageSize, total);
    }
}
