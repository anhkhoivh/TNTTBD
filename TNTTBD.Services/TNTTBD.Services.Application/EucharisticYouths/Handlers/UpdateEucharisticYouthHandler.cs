using MediatR;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Create Eucharistic Youth handler
/// </summary>
public class UpdateEucharisticYouthHandler : IRequestHandler<UpdateEucharisticYouthCommand>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public UpdateEucharisticYouthHandler(IEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        List<PatronSaint>? patronSaints = command.PatronSaints != null ? command.PatronSaints.Select(p => p.ToDomain()).ToList() : null;
        List<SpiritualMilestone>? milestones = command.Milestones != null ? command.Milestones.Select(p => p.ToDomain()).ToList() : null;
        await _repository.Update(command.EucharisticYouthId, fullName, command.DateOfBirth, command.Address, command.IsMale, patronSaints, milestones, cancellationToken);
    }

    #endregion
}