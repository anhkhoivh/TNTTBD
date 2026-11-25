using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

/// <summary>
/// Create Session handler
/// </summary>
public class MarkAttendanceListHandler : IRequestHandler<MarkAttendanceListCommand>
{
    #region Fields

    private readonly ICatechismSessionRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public MarkAttendanceListHandler(ICatechismSessionRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task Handle(MarkAttendanceListCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessCatechistAsync(command.MarkedByCatechistId, cancellationToken))
        {
            throw new ForbiddenException();
        }

        await _repository.MarkAttendanceList(
            command.SessionId, 
            command.AttendanceItems.Select(a => (a.CatechismClassEucharisticYouthId, a.AttendanceStatus)), 
            command.MarkedByCatechistId, cancellationToken);
    }

    #endregion
}