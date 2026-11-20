using System.Reflection;
using EcommerceBackend.Application.Models;
using FluentValidation;
using MediatR;

namespace EcommerceBackend.Application.Abstractions;

/// <summary>
/// Pipeline behavior that runs every FluentValidation validator registered for the request
/// and short-circuits with a validation <see cref="Error"/> before the handler executes.
/// </summary>
internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    // Result.Failure / Result<T>.Failure, resolved once per closed generic type.
    private static readonly MethodInfo FailureFactory = typeof(TResponse).GetMethod(
        nameof(Result.Failure),
        BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly,
        [typeof(Error)])!;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, ct)));

        var errors = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .GroupBy(f => ToCamelCase(f.PropertyName))
            .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).Distinct().ToArray());

        if (errors.Count == 0)
            return await next();

        return (TResponse)FailureFactory.Invoke(null, [Error.Validation(errors)])!;
    }

    private static string ToCamelCase(string name) =>
        string.IsNullOrEmpty(name) ? name : char.ToLowerInvariant(name[0]) + name[1..];
}
