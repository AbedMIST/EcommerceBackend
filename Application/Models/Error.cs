namespace EcommerceBackend.Application.Models;

public enum ErrorType { Validation, NotFound, Conflict }

public sealed record Error(
    ErrorType Type,
    string Code,
    string Description,
    IReadOnlyDictionary<string, string[]>? ValidationErrors = null)
{
    public static Error NotFound(string code, string description) => new(ErrorType.NotFound, code, description);
    public static Error Conflict(string code, string description) => new(ErrorType.Conflict, code, description);
    public static Error Validation(IReadOnlyDictionary<string, string[]> errors) =>
        new(ErrorType.Validation, "Validation.Failed", "One or more validation errors occurred.", errors);
}
