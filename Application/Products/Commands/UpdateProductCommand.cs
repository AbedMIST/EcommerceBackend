using EcommerceBackend.Application.DTOs;
using EcommerceBackend.Application.Abstractions;
using EcommerceBackend.Application.Models;
using EcommerceBackend.Domain.Entities;
using FluentValidation;

namespace EcommerceBackend.Application.Products.Commands;

public sealed record UpdateProductCommand(Guid Id, string Name, string? Description, string Sku, decimal Price, int StockQuantity)
    : ICommand<Result<ProductResponse>>;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).ValidName();
        RuleFor(x => x.Description).ValidDescription();
        RuleFor(x => x.Sku).ValidSku();
        RuleFor(x => x.Price).ValidPrice();
        RuleFor(x => x.StockQuantity).ValidStock();
    }
}

internal sealed class UpdateProductCommandHandler(IProductRepository repository)
    : ICommandHandler<UpdateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(command.Id, ct);
        if (product is null)
            return ProductErrors.NotFound(command.Id);

        var sku = Product.NormalizeSku(command.Sku);
        if (await repository.SkuExistsAsync(sku, excludeId: command.Id, ct))
            return ProductErrors.DuplicateSku(sku);

        product.Update(command.Name, command.Description, command.Sku, command.Price, command.StockQuantity);
        await repository.UpdateAsync(product, ct);

        return ProductResponse.FromEntity(product);
    }
}
