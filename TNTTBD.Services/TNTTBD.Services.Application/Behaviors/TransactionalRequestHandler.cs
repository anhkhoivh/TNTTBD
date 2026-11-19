using TNTTBD.Services.Application.Abstractions;

namespace TNTTBD.Services.Application.Behaviors;

/// <summary>
/// Decorates a handler so it runs inside a unit-of-work transaction.
/// </summary>
public sealed class TransactionalRequestHandler<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    IUnitOfWork unitOfWork) : IRequestHandler<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        => TransactionRunner.RunAsync(unitOfWork, () => inner.Handle(request, cancellationToken));
}