using EcommerceBackend.Application.DTOs;
using EcommerceBackend.Application.Abstractions;
using EcommerceBackend.Application.Models;

namespace EcommerceBackend.Application.Products.Queries;

public sealed record GetProductsQuery(int Page = 1, int PageSize = 20, string? Search = null)
    : IQuery<Result<PagedResult<ProductResponse>>>;

internal sealed class GetProductsQueryHandler(IProductReadRepository readRepository)
    : IQueryHandler<GetProductsQuery, Result<PagedResult<ProductResponse>>>
{
    public const int MaxPageSize = 100;

    public async Task<Result<PagedResult<ProductResponse>>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);
        return await readRepository.GetPagedAsync(page, pageSize, query.Search?.Trim(), ct);
    }
}
