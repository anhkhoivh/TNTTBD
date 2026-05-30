using MediatR;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class UpdateCatechistClassAssignmentHandler : IRequestHandler<UpdateCatechistClassAssignmentCommand>
{
    private readonly ICatechismClassCatechistRepository _repository;

    public UpdateCatechistClassAssignmentHandler(ICatechismClassCatechistRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateCatechistClassAssignmentCommand command, CancellationToken cancellationToken)
    {
        await _repository.Update(command.Id, command.Role, command.Note, command.LeftAt, cancellationToken);
    }
}
