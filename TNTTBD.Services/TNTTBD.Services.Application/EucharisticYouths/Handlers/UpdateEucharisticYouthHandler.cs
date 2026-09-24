using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Create Eucharistic Youth handler
/// </summary>
public class UpdateEucharisticYouthHandler : IRequestHandler<UpdateEucharisticYouthCommand, bool>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public UpdateEucharisticYouthHandler(IEucharisticYouthRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(UpdateEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        List<PatronSaint>? patronSaints = command.PatronSaints != null ? command.PatronSaints.Select(p => p.ToDomain()).ToList() : null;
        List<SpiritualMilestone>? milestones = command.Milestones != null ? command.Milestones.Select(p => p.ToDomain()).ToList() : null;
        await _repository.Update(command.EucharisticYouthId, fullName, command.DateOfBirth, command.Address, command.IsMale, patronSaints, milestones, cancellationToken);
        return true;
    }

    #endregion
}