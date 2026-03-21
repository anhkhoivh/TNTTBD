using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthMilestoneHandler : IRequestHandler<RemoveEucharisticYouthMilestoneCommand>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="unitOfWork"></param>
    public RemoveEucharisticYouthMilestoneHandler(IEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(RemoveEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        await _repository.RemoveMilestone(command.EucharisticYouthId, command.MilestoneId, cancellationToken);
    }

    #endregion
}