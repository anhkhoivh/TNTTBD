using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class UpdateCatechistClassStatusHandler(ICatechismClassCatechistRepository repository, IAuthorizationContext authContext) : IRequestHandler<UpdateCatechistClassStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateCatechistClassStatusCommand command, CancellationToken cancellationToken)
    {
        if (!await authContext.IsAdminAsync(cancellationToken))
        {
            return false;
        }

        if (!Enum.IsDefined(command.Status))
        {
            throw new DomainException(DomainErrors.ValidationRequired);
        }

        CatechismClassCatechist assignment = await repository.GetAsync(command.Id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        if (command.Status == EnrollmentStatus.Active
            && assignment.Status != EnrollmentStatus.Active
            && assignment.Role == CatechismClassCatechistRole.Main
            && await repository.HasActiveMainCatechistAsync(assignment.CatechismClassId, cancellationToken))
        {
            throw new DomainException(DomainErrors.MainCatechistAlreadyActive);
        }

        await repository.UpdateStatus(command.Id, command.Status, cancellationToken);
        return true;
    }
}
