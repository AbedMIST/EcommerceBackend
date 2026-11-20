using EcommerceBackend.Application.Models;

namespace EcommerceBackend.Application.Products;

public static class ProductErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Product.NotFound", $"Product with id '{id}' was not found.");

    public static Error DuplicateSku(string sku) =>
        Error.Conflict("Product.DuplicateSku", $"A product with SKU '{sku}' already exists.");
}
