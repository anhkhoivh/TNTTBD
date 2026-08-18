using MediatR;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class CreateCatechismClassHandler : IRequestHandler<CreateCatechismClassCommand, Guid>
{
    #region Fields

    private readonly ICatechismClassRepository _repository;

    #endregion

    #region Constructors

    public CreateCatechismClassHandler(ICatechismClassRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(CreateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        CatechismClass newClass = new(Guid.NewGuid(), command.CatechismYearId, command.Name, command.Code);
        await _repository.AddAsync(newClass, cancellationToken);
        return newClass.Id;
    }

    #endregion
}