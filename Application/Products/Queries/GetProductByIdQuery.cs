using EcommerceBackend.Application.DTOs;
using EcommerceBackend.Application.Abstractions;
using EcommerceBackend.Application.Models;

namespace EcommerceBackend.Application.Products.Queries;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<Result<ProductResponse>>;

internal sealed class GetProductByIdQueryHandler(IProductReadRepository readRepository)
    : IQueryHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await readRepository.GetByIdAsync(query.Id, ct);
        return product is null ? ProductErrors.NotFound(query.Id) : product;
    }
}
