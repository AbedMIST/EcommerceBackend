namespace EcommerceBackend.Domain.Common;

/// <summary>Thrown when an operation would violate a domain invariant.</summary>
public sealed class DomainException(string message) : Exception(message);
