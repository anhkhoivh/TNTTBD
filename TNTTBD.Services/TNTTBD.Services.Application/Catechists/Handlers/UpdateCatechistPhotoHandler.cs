using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class UpdateCatechistPhotoHandler : IRequestHandler<UpdateCatechistPhotoCommand, bool>
{
    private readonly ICatechistRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public UpdateCatechistPhotoHandler(ICatechistRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<bool> Handle(UpdateCatechistPhotoCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanManageCatechistPhotosAsync(command.CatechistId, cancellationToken))
        {
            return false;
        }

        await _repository.UpdatePhoto(command.CatechistId, command.PhotoId, command.Purpose, command.Visibility, cancellationToken);
        return true;
    }
}
