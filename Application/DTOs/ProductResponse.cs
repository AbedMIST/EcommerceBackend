using EcommerceBackend.Domain.Entities;

namespace EcommerceBackend.Application.DTOs;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string? Description,
    string Sku,
    decimal Price,
    int StockQuantity,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static ProductResponse FromEntity(Product p) =>
        new(p.Id, p.Name, p.Description, p.Sku, p.Price, p.StockQuantity, p.CreatedAt, p.UpdatedAt);
}
