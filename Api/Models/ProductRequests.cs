namespace EcommerceBackend.Api.Models;

public sealed record CreateProductRequest(string Name, string? Description, string Sku, decimal Price, int StockQuantity);

public sealed record UpdateProductRequest(string Name, string? Description, string Sku, decimal Price, int StockQuantity);
