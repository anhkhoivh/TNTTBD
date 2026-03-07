using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveMilestoneHandler : IRequestHandler<RemoveMilestoneCommand>
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
    public RemoveMilestoneHandler(IEucharisticYouthRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Methods

    public async Task Handle(RemoveMilestoneCommand command, CancellationToken cancellationToken)
    {
        await _repository.RemoveMilestone(command.EucharisticYouthId, command.MilestoneId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion
}