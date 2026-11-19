using TNTTBD.Services.Application.Abstractions;
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
    private readonly IEucharisticYouthContactRepository _youthContactRepository;

    #endregion

    #region Constructors

    public AddEucharisticYouthMilestoneHandler(
        IEucharisticYouthRepository repository,
        IAuthorizationContext authContext,
        IEucharisticYouthContactRepository youthContactRepository)
    {
        _repository = repository;
        _authContext = authContext;
        _youthContactRepository = youthContactRepository;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(AddEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        SpiritualMilestone milestone = new(Guid.NewGuid(), command.Type, command.Date, command.DocumentCode, command.Place, command.Note, command.SponsorContactId, command.Celebrant);
        await _repository.AddMilestone(command.EucharisticYouthId, milestone, cancellationToken);
        await _youthContactRepository.LinkGodparentAsync(command.EucharisticYouthId, milestone.Type, milestone.SponsorContactId, cancellationToken);
        return true;
    }

    #endregion
}