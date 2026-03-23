using MediatR;
using TNTTBD.Services.Application.CatechismYears.Commands;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismYears.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class CreateCatechismYearHandler : IRequestHandler<CreateCatechismYearCommand, Guid>
{
    #region Fields

    private readonly ICatechismYearRepository _repository;

    #endregion

    #region Constructors

    public CreateCatechismYearHandler(ICatechismYearRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(CreateCatechismYearCommand command, CancellationToken cancellationToken)
    {
        CatechismYear year = new(Guid.NewGuid(), command.StartDate, command.EndDate, command.Theme);
        await _repository.AddAsync(year, cancellationToken);
        return year.Id;
    }

    #endregion
}