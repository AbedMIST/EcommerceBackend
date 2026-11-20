using EcommerceBackend.Domain.Entities;
using FluentValidation;

namespace EcommerceBackend.Application.Products;

/// <summary>Validation rules shared by the create and update commands.</summary>
internal static class ProductRules
{
    public static IRuleBuilderOptions<T, string> ValidName<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("Name is required.")
            .MaximumLength(Product.NameMaxLength).WithMessage($"Name must be at most {Product.NameMaxLength} characters.");

    public static IRuleBuilderOptions<T, string?> ValidDescription<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(Product.DescriptionMaxLength).WithMessage($"Description must be at most {Product.DescriptionMaxLength} characters.");

    public static IRuleBuilderOptions<T, string> ValidSku<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(Product.SkuMaxLength).WithMessage($"SKU must be at most {Product.SkuMaxLength} characters.");

    public static IRuleBuilderOptions<T, decimal> ValidPrice<T>(this IRuleBuilder<T, decimal> rule) =>
        rule.GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.")
            .PrecisionScale(18, 2, ignoreTrailingZeros: true).WithMessage("Price must have at most 2 decimal places.");

    public static IRuleBuilderOptions<T, int> ValidStock<T>(this IRuleBuilder<T, int> rule) =>
        rule.GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
}
