using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class AssignCatechistToClassHandler(ICatechismClassCatechistRepository repository, IAuthorizationContext authContext) : IRequestHandler<AssignCatechistToClassCommand, Guid?>
{
    public async Task<Guid?> Handle(AssignCatechistToClassCommand command, CancellationToken cancellationToken)
    {
        if (!await authContext.IsAdminAsync(cancellationToken))
        {
            return null;
        }
        if (command.Role == CatechismClassCatechistRole.Main)
        {
            var hasMain = await repository.HasActiveMainCatechistAsync(command.CatechismClassId, cancellationToken);
            if (hasMain)
                throw new InvalidOperationException("Class already has an active primary catechist.");
        }

        var id = Guid.NewGuid();
        var domain = new CatechismClassCatechist(
            id,
            command.CatechistId,
            command.CatechismClassId,
            command.Role,
            DateTimeOffset.UtcNow,
            null,
            EnrollmentStatus.Active,
            command.Note);

        await repository.AddAsync(domain, cancellationToken);
        return id;
    }
}
