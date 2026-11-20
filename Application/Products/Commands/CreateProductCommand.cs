using EcommerceBackend.Application.DTOs;
using EcommerceBackend.Application.Abstractions;
using EcommerceBackend.Application.Models;
using EcommerceBackend.Domain.Entities;
using FluentValidation;

namespace EcommerceBackend.Application.Products.Commands;

public sealed record CreateProductCommand(string Name, string? Description, string Sku, decimal Price, int StockQuantity, bool IsDiscounted = false)
    : ICommand<Result<ProductResponse>>;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).ValidName();
        RuleFor(x => x.Description).ValidDescription();
        RuleFor(x => x.Sku).ValidSku();
        RuleFor(x => x.Price).ValidPrice();
        RuleFor(x => x.StockQuantity).ValidStock();
    }
}

internal sealed class CreateProductCommandHandler(IProductRepository repository)
    : ICommandHandler<CreateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var sku = Product.NormalizeSku(command.Sku);
        if (await repository.SkuExistsAsync(sku, ct: ct))
            return ProductErrors.DuplicateSku(sku);

        var product = Product.Create(command.Name, command.Description, command.Sku, command.Price, command.StockQuantity, command.IsDiscounted);
        await repository.AddAsync(product, ct);

        return ProductResponse.FromEntity(product);
    }
}
