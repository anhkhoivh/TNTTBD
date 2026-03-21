using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddEucharisticYouthPatronSaintHandler : IRequestHandler<AddEucharisticYouthPatronSaintCommand>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public AddEucharisticYouthPatronSaintHandler(IEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(AddEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        PatronSaint patronSaint = new(command.SaintName, command.Day, command.Month);
        await _repository.AddPatronSaint(command.EucharisticYouthId, patronSaint, cancellationToken);
    }

    #endregion
}