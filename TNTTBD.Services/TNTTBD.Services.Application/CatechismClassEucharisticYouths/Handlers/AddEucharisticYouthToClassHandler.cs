using MediatR;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class AddEucharisticYouthToClassHandler : IRequestHandler<AddEucharisticYouthToClassCommand, Guid>
{
    #region Fields

    private readonly ICatechismClassEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public AddEucharisticYouthToClassHandler(ICatechismClassEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(AddEucharisticYouthToClassCommand command, CancellationToken cancellationToken)
    {
        CatechismClassEucharisticYouth domain = new(Guid.NewGuid(), command.EucharisticYouthId, command.CatechismClassId, DateTimeOffset.Now.ToUniversalTime(), null, EnrollmentStatus.Active, null);
        await _repository.AddAsync(domain, cancellationToken);
        return domain.Id;
    }

    #endregion
}