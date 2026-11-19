using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechistHandler : IRequestHandler<UpdateCatechistCommand, bool>
{
    #region Fields

    private readonly ICatechistRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public UpdateCatechistHandler(ICatechistRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(UpdateCatechistCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessCatechistAsync(command.CatechistId, cancellationToken))
        {
            return false;
        }

        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        List<PatronSaint>? patronSaints = command.PatronSaints != null ? command.PatronSaints.Select(p => p.ToDomain()).ToList() : null;
        await _repository.Update(command.CatechistId, fullName, command.DateOfBirth, command.Address, command.IsMale, command.PhoneNumber, patronSaints, cancellationToken);
        return true;
    }

    #endregion
}