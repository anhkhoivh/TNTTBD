using MediatR;

namespace TNTTBD.Services.Application.Exams.Commands;

public record RemoveExamCommand(Guid Id) : IRequest;
