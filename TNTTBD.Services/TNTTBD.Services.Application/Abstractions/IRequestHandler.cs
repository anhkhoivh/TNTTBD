namespace TNTTBD.Services.Application.Abstractions;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest>
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}
