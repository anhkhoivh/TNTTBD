using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class UpdateEucharisticYouthMilestoneHandler : IRequestHandler<UpdateEucharisticYouthMilestoneCommand, bool>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;
    private readonly IContactRepository _contactRepository;
    private readonly IEucharisticYouthContactRepository _youthContactRepository;

    #endregion

    #region Constructors

    public UpdateEucharisticYouthMilestoneHandler(
        IEucharisticYouthRepository repository,
        IAuthorizationContext authContext,
        IContactRepository contactRepository,
        IEucharisticYouthContactRepository youthContactRepository)
    {
        _repository = repository;
        _authContext = authContext;
        _contactRepository = contactRepository;
        _youthContactRepository = youthContactRepository;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(UpdateEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        await _contactRepository.EnsureSponsorsExistAsync([command.SponsorContactId], cancellationToken);
        // Type is a placeholder (it keeps the sponsor); the repository applies the update to the stored milestone.
        SpiritualMilestone milestone = new(command.MilestoneId, MilestoneType.Baptism, command.Date, command.DocumentCode, command.Place, command.Note, command.SponsorContactId, command.Celebrant);
        MilestoneType actualType = await _repository.UpdateMilestone(command.EucharisticYouthId, milestone, cancellationToken);
        await _youthContactRepository.LinkGodparentAsync(command.EucharisticYouthId, actualType, command.SponsorContactId, new HashSet<Guid>(), cancellationToken);
        return true;
    }

    #endregion
}