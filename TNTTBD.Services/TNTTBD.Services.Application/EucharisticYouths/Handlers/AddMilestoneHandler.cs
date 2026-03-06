using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddMilestoneHandler
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IUnitOfWork  _unitOfWork;

    #endregion

    #region Constructors

    public AddMilestoneHandler(IEucharisticYouthRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Methods

    public async Task Handle(AddMilestoneCommand command, CancellationToken cancellationToken)
    {
        SpiritualMilestone milestone = new(Guid.NewGuid(), command.Type, command.Date, command.DocumentCode, command.Place, command.Note);
        await _repository.AddMilestone(command.EucharisticYouthId, milestone, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion
}