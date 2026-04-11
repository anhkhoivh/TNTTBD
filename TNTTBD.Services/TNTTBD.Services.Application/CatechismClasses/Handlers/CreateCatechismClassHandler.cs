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

    private readonly ICatechismYearRepository _repository;

    #endregion

    #region Constructors

    public CreateCatechismClassHandler(ICatechismYearRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(CreateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        CatechismYear year = new(Guid.NewGuid(), command.StartDate, command.EndDate, command.Theme);
        await _repository.AddAsync(year, cancellationToken);
        return year.Id;
    }

    #endregion
}