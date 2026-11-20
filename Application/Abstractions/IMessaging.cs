using MediatR;

namespace EcommerceBackend.Application.Abstractions;

/// <summary>A request that changes state.</summary>
public interface ICommand<out TResponse> : IRequest<TResponse>;

/// <summary>A request that only reads state and never has side effects.</summary>
public interface IQuery<out TResponse> : IRequest<TResponse>;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;
