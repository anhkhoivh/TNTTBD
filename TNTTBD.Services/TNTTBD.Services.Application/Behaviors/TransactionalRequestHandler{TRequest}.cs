using TNTTBD.Services.Application.Abstractions;

namespace TNTTBD.Services.Application.Behaviors;

/// <summary>
/// Decorates a handler without a response so it runs inside a unit-of-work transaction.
/// </summary>
public sealed class TransactionalRequestHandler<TRequest>(
    IRequestHandler<TRequest> inner,
    IUnitOfWork unitOfWork) : IRequestHandler<TRequest>
{
    public Task Handle(TRequest request, CancellationToken cancellationToken)
        => TransactionRunner.RunAsync(unitOfWork, async () =>
        {
            await inner.Handle(request, cancellationToken);
            return true;
        });
}