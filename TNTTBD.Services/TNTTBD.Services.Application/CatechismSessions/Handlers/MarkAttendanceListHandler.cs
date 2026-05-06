using MediatR;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

/// <summary>
/// Create Session handler
/// </summary>
public class MarkAttendanceListHandler : IRequestHandler<MarkAttendanceListCommand>
{
    #region Fields

    private readonly ICatechismSessionRepository _repository;

    #endregion

    #region Constructors

    public MarkAttendanceListHandler(ICatechismSessionRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(MarkAttendanceListCommand command, CancellationToken cancellationToken)
    {
        await _repository.MarkAttendanceList(
            command.SessionId, 
            command.AttendanceItems.Select(a => (a.CatechismClassEucharisticYouthId, a.AttendanceStatus)), 
            command.MarkedByCatechistId, cancellationToken);
    }

    #endregion
}