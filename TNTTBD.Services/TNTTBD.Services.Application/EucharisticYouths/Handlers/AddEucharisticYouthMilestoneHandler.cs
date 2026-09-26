using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddEucharisticYouthMilestoneHandler : IRequestHandler<AddEucharisticYouthMilestoneCommand, bool>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public AddEucharisticYouthMilestoneHandler(IEucharisticYouthRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(AddEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        SpiritualMilestone milestone = new(Guid.NewGuid(), command.Type, command.Date, command.DocumentCode, command.Place, command.Note);
        await _repository.AddMilestone(command.EucharisticYouthId, milestone, cancellationToken);
        return true;
    }

    #endregion
}