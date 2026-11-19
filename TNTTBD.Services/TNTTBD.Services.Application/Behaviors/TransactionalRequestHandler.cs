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

internal static class TransactionRunner
{
    public static async Task<TResponse> RunAsync<TResponse>(IUnitOfWork unitOfWork, Func<Task<TResponse>> next)
    {
        await unitOfWork.BeginTransactionAsync();

        try
        {
            TResponse response = await next();

            await unitOfWork.CommitTransactionAsync();

            return response;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
