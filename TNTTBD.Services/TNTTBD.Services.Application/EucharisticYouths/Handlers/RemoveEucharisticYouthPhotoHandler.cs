using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthPhotoHandler : IRequestHandler<RemoveEucharisticYouthPhotoCommand, string?>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public RemoveEucharisticYouthPhotoHandler(IEucharisticYouthRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<string?> Handle(RemoveEucharisticYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanManageYouthPhotosAsync(command.EucharisticYouthId, cancellationToken))
        {
            return null;
        }

        return await _repository.RemovePhoto(command.EucharisticYouthId, command.PhotoId, cancellationToken);
    }
}
