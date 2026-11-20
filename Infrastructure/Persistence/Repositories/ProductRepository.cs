using EcommerceBackend.Application.Abstractions;
using EcommerceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Infrastructure.Persistence.Repositories;

internal sealed class ProductRepository(AppDbContext db) : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) 
    {
        return db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public Task<bool> SkuExistsAsync(string normalizedSku, Guid? excludeId = null, CancellationToken ct = default)
    {
        return db.Products.AnyAsync(p => p.Sku == normalizedSku && (excludeId == null || p.Id != excludeId), ct);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        // A tracked product (loaded via GetByIdAsync) already has its changes detected, so just save.
        // An untracked one must be attached first, otherwise SaveChanges would silently do nothing.
        if (db.Entry(product).State == EntityState.Detached)
            db.Products.Update(product);

        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Product product, CancellationToken ct = default)
    {
        db.Products.Remove(product);
        await db.SaveChangesAsync(ct);
    }
}
