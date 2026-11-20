using EcommerceBackend.Domain.Entities;

namespace EcommerceBackend.Application.Abstractions;

/// <summary>Write-side repository: loads and persists the <see cref="Product"/> aggregate for commands.</summary>
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> SkuExistsAsync(string normalizedSku, Guid? excludeId = null, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task RemoveAsync(Product product, CancellationToken ct = default);
}
