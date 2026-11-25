namespace TNTTBD.Services.Application.Abstractions;

public interface IRequestHandler<in TRequest>
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}