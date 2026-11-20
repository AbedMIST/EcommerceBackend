using EcommerceBackend.Domain.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace EcommerceBackend.Api.Infrastructure;

/// <summary>Safety net: a domain invariant violation that slipped past validation becomes a 400.</summary>
internal sealed class DomainExceptionHandler(IProblemDetailsService problemDetails) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not DomainException)
            return false;

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return await problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails = { Status = 400, Title = "Bad Request", Detail = exception.Message },
        });
    }
}
