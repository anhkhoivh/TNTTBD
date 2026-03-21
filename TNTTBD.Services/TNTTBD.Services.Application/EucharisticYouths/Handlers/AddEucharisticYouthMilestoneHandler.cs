using MediatR;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddEucharisticYouthMilestoneHandler : IRequestHandler<AddEucharisticYouthMilestoneCommand>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public AddEucharisticYouthMilestoneHandler(IEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(AddEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        SpiritualMilestone milestone = new(Guid.NewGuid(), command.Type, command.Date, command.DocumentCode, command.Place, command.Note);
        await _repository.AddMilestone(command.EucharisticYouthId, milestone, cancellationToken);
    }

    #endregion
}