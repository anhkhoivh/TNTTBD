using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class UpdateMilestoneHandler : IRequestHandler<UpdateMilestoneCommand>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="unitOfWork"></param>
    public UpdateMilestoneHandler(IEucharisticYouthRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateMilestoneCommand command, CancellationToken cancellationToken)
    {
        SpiritualMilestone milestone = new(command.MilestoneId, MilestoneType.Baptism, command.Date, command.DocumentCode, command.Place, command.Note);
        await _repository.UpdateMilestone(command.EucharisticYouthId, milestone, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion
}