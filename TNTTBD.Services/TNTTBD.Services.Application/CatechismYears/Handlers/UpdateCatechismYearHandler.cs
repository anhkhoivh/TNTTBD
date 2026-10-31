using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.CatechismYears.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismYears.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismYearHandler : IRequestHandler<UpdateCatechismYearCommand>
{
    #region Fields

    private readonly ICatechismYearRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public UpdateCatechismYearHandler(ICatechismYearRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateCatechismYearCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanWriteYearsAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }

        await _repository.Update(command.CatechismYearId, command.StartDate, command.EndDate, command.Theme, cancellationToken);
    }

    #endregion
}