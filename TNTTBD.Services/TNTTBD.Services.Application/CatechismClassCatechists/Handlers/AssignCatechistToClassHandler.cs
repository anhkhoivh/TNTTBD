using MediatR;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class AssignCatechistToClassHandler : IRequestHandler<AssignCatechistToClassCommand, Guid>
{
    private readonly ICatechismClassCatechistRepository _repository;

    public AssignCatechistToClassHandler(ICatechismClassCatechistRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AssignCatechistToClassCommand command, CancellationToken cancellationToken)
    {
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

        await _repository.AddAsync(domain, cancellationToken);
        return id;
    }
}
