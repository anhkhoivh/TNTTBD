using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismClassHandler(ICatechismClassRepository repository, IAuthorizationContext authContext) : IRequestHandler<UpdateCatechismClassCommand, bool>
{
    #region Methods

    public async Task<bool> Handle(UpdateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        if (!await authContext.CanAccessClassAsync(command.CatechismClassId, cancellationToken))
        {
            return false;
        }
        await repository.Update(command.CatechismClassId, command.ClassCode, command.ClassName, cancellationToken);
        return true;
    }

    #endregion
}