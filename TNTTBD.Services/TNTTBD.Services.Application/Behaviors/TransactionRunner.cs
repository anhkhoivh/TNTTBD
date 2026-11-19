using TNTTBD.Services.Application.Abstractions;

namespace TNTTBD.Services.Application.Behaviors;

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