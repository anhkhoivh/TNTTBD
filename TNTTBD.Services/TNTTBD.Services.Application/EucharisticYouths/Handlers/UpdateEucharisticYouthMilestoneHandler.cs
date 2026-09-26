using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class UpdateEucharisticYouthMilestoneHandler : IRequestHandler<UpdateEucharisticYouthMilestoneCommand, bool>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="unitOfWork"></param>
    public UpdateEucharisticYouthMilestoneHandler(IEucharisticYouthRepository repository, IUnitOfWork unitOfWork, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(UpdateEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        SpiritualMilestone milestone = new(command.MilestoneId, MilestoneType.Baptism, command.Date, command.DocumentCode, command.Place, command.Note);
        await _repository.UpdateMilestone(command.EucharisticYouthId, milestone, cancellationToken);
        return true;
    }

    #endregion
}