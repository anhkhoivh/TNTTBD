using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class UpdateEucharisticYouthPhotoHandler : IRequestHandler<UpdateEucharisticYouthPhotoCommand, bool>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public UpdateEucharisticYouthPhotoHandler(IEucharisticYouthRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<bool> Handle(UpdateEucharisticYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanManageYouthPhotosAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        await _repository.UpdatePhoto(command.EucharisticYouthId, command.PhotoId, command.Purpose, cancellationToken);
        return true;
    }
}
