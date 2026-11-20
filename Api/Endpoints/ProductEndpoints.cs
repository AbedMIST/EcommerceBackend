using EcommerceBackend.Api.Models;
using EcommerceBackend.Application.DTOs;
using EcommerceBackend.Api.Infrastructure;
using EcommerceBackend.Application.Models;
using EcommerceBackend.Application.Products;
using EcommerceBackend.Application.Products.Commands;
using EcommerceBackend.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EcommerceBackend.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", GetAll)
            .WithName("GetProducts")
            .WithSummary("List products (paged, optional search by name or SKU)");

        group.MapGet("/{id:guid}", GetById)
            .WithName("GetProductById")
            .WithSummary("Get a product by id")
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", Create)
            .WithName("CreateProduct")
            .WithSummary("Create a product")
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapPut("/{id:guid}", Update)
            .WithName("UpdateProduct")
            .WithSummary("Replace a product")
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapDelete("/{id:guid}", Delete)
            .WithName("DeleteProduct")
            .WithSummary("Delete a product")
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<Results<Ok<PagedResult<ProductResponse>>, ProblemHttpResult>> GetAll(
        ISender sender, CancellationToken ct, int page = 1, int pageSize = 20, string? search = null)
    {
        var result = await sender.Send(new GetProductsQuery(page, pageSize, search), ct);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error!.ToProblem();
    }

    private static async Task<Results<Ok<ProductResponse>, ProblemHttpResult>> GetById(
        Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetProductByIdQuery(id), ct);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error!.ToProblem();
    }

    private static async Task<Results<CreatedAtRoute<ProductResponse>, ProblemHttpResult>> Create(
        CreateProductRequest request, ISender sender, CancellationToken ct)
    {
        var command = new CreateProductCommand(request.Name, request.Description, request.Sku, request.Price, request.StockQuantity, request.IsDiscounted);
        var result = await sender.Send(command, ct);
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(result.Value, "GetProductById", new { id = result.Value.Id })
            : result.Error!.ToProblem();
    }

    private static async Task<Results<Ok<ProductResponse>, ProblemHttpResult>> Update(
        Guid id, UpdateProductRequest request, ISender sender, CancellationToken ct)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Description, request.Sku, request.Price, request.StockQuantity, request.IsDiscounted);
        var result = await sender.Send(command, ct);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error!.ToProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> Delete(
        Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new DeleteProductCommand(id), ct);
        return result.IsSuccess ? TypedResults.NoContent() : result.Error!.ToProblem();
    }
}
