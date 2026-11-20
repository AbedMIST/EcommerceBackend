using EcommerceBackend.Domain.Common;

namespace EcommerceBackend.Domain.Entities;

public sealed class Product : Entity
{
    public const int NameMaxLength = 200;
    public const int DescriptionMaxLength = 2000;
    public const int SkuMaxLength = 64;

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Sku { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    // Required by EF Core.
    private Product() { }

    public static Product Create(string name, string? description, string sku, decimal price, int stockQuantity)
    {
        var product = new Product();
        product.Apply(name, description, sku, price, stockQuantity);
        return product;
    }

    public void Update(string name, string? description, string sku, decimal price, int stockQuantity)
    {
        Apply(name, description, sku, price, stockQuantity);
        UpdatedAt = DateTime.UtcNow;
    }

    private void Apply(string name, string? description, string sku, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > NameMaxLength)
            throw new DomainException($"Name is required and must be at most {NameMaxLength} characters.");
        if (description is not null && description.Length > DescriptionMaxLength)
            throw new DomainException($"Description must be at most {DescriptionMaxLength} characters.");
        if (string.IsNullOrWhiteSpace(sku) || sku.Trim().Length > SkuMaxLength)
            throw new DomainException($"SKU is required and must be at most {SkuMaxLength} characters.");
        if (price < 0)
            throw new DomainException("Price cannot be negative.");
        if (stockQuantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Sku = NormalizeSku(sku);
        Price = price;
        StockQuantity = stockQuantity;
    }

    public static string NormalizeSku(string sku) => sku.Trim().ToUpperInvariant();
}
