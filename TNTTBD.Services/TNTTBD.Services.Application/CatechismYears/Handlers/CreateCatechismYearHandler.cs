using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
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
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public CreateCatechismYearHandler(ICatechismYearRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(CreateCatechismYearCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanWriteYearsAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }

        CatechismYear year = new(Guid.NewGuid(), command.StartDate, command.EndDate, command.Theme);
        await _repository.AddAsync(year, cancellationToken);
        return year.Id;
    }

    #endregion
}