using MediatR;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

/// <summary>
/// Create Session handler
/// </summary>
public class MarkAttendanceHandler : IRequestHandler<MarkAttendanceCommand>
{
    #region Fields

    private readonly ICatechismSessionRepository _repository;

    #endregion

    #region Constructors

    public MarkAttendanceHandler(ICatechismSessionRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(MarkAttendanceCommand command, CancellationToken cancellationToken)
    {
        await _repository.MarkAttendance(command.SessionId, command.AttendanceItem.CatechismClassEucharisticYouthId, command.AttendanceItem.AttendanceStatus, command.MarkedByCatechistId, cancellationToken);
    }

    #endregion
}