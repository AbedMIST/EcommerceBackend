using EcommerceBackend.Application.Abstractions;
using EcommerceBackend.Application.Models;

namespace EcommerceBackend.Application.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : ICommand<Result>;

internal sealed class DeleteProductCommandHandler(IProductRepository repository)
    : ICommandHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(command.Id, ct);
        if (product is null)
            return Result.Failure(ProductErrors.NotFound(command.Id));

        await repository.RemoveAsync(product, ct);
        return Result.Success();
    }
}
