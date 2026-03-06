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
        EucharisticYouth? youth = await _repository.GetByIdAsync(command.EucharisticYouthId, cancellationToken);

        if (youth == null)
            throw new Exception(DomainErrors.ValidationNotFound);

        youth.RemoveMilestone(command.MilestoneId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion
}