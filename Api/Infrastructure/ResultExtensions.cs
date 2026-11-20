using EcommerceBackend.Application.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EcommerceBackend.Api.Infrastructure;

internal static class ResultExtensions
{
    /// <summary>Converts an application <see cref="Error"/> into an RFC 7807 problem response.</summary>
    public static ProblemHttpResult ToProblem(this Error error)
    {
        if (error.Type == ErrorType.Validation)
        {
            return TypedResults.Problem(new HttpValidationProblemDetails(
                error.ValidationErrors?.ToDictionary() ?? [])
            {
                Status = StatusCodes.Status400BadRequest,
                Title = error.Description,
            });
        }

        var (status, title) = error.Type switch
        {
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Not Found"),
            ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error"),
        };

        return TypedResults.Problem(
            statusCode: status,
            title: title,
            detail: error.Description,
            extensions: new Dictionary<string, object?> { ["code"] = error.Code });
    }
}
