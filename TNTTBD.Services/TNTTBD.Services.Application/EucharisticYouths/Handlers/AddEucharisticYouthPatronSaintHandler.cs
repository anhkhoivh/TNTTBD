using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddEucharisticYouthPatronSaintHandler : IRequestHandler<AddEucharisticYouthPatronSaintCommand, bool>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public AddEucharisticYouthPatronSaintHandler(IEucharisticYouthRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(AddEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        PatronSaint patronSaint = new(command.SaintName, command.Day, command.Month);
        await _repository.AddPatronSaint(command.EucharisticYouthId, patronSaint, cancellationToken);
        return true;
    }

    #endregion
}