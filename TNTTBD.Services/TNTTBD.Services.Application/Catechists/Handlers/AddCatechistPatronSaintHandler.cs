using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddCatechistPatronSaintHandler : IRequestHandler<AddCatechistPatronSaintCommand, bool>
{
    #region Fields

    private readonly ICatechistRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public AddCatechistPatronSaintHandler(ICatechistRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(AddCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessCatechistAsync(command.CatechistId, cancellationToken))
        {
            return false;
        }

        PatronSaint patronSaint = new(command.SaintName, command.Day, command.Month);
        await _repository.AddPatronSaint(command.CatechistId, patronSaint, cancellationToken);
        return true;
    }

    #endregion
}