using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RemoveCatechistPhotoHandler : IRequestHandler<RemoveCatechistPhotoCommand, string?>
{
    private readonly ICatechistRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public RemoveCatechistPhotoHandler(ICatechistRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<string?> Handle(RemoveCatechistPhotoCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanManageCatechistPhotosAsync(command.CatechistId, cancellationToken))
        {
            return null;
        }

        return await _repository.RemovePhoto(command.CatechistId, command.PhotoId, cancellationToken);
    }
}
