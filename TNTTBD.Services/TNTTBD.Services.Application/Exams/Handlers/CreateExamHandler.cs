using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class CreateExamHandler : IRequestHandler<CreateExamCommand, Guid>
{
    private readonly IExamRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public CreateExamHandler(IExamRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<Guid> Handle(CreateExamCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessClassAsync(command.CatechismClassId, cancellationToken))
        {
            throw new ForbiddenException();
        }

        Exam exam = new(Guid.NewGuid(), command.CatechismClassId, command.ExamDate, command.Title, []);
        await _repository.AddAsync(exam, cancellationToken);
        return exam.Id;
    }
}
