using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class CreateCatechismClassHandler(ICatechismClassRepository repository, IAuthorizationContext authContext) : IRequestHandler<CreateCatechismClassCommand, Guid?>
{
    #region Methods

    public async Task<Guid?> Handle(CreateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        if (!await authContext.IsAdminAsync(cancellationToken))
        {
            return null;
        }
        CatechismClass newClass = new(Guid.NewGuid(), command.CatechismYearId, command.Name, command.Code);
        await repository.AddAsync(newClass, cancellationToken);
        return newClass.Id;
    }

    #endregion
}