using MediatR;
using TNTTBD.Services.Application.Catechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechistHandler : IRequestHandler<UpdateCatechistCommand>
{
    #region Fields

    private readonly ICatechistRepository _repository;

    #endregion

    #region Constructors

    public UpdateCatechistHandler(ICatechistRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateCatechistCommand command, CancellationToken cancellationToken)
    {
        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        await _repository.UpdateCatechist(command.CatechistId, fullName, command.DateOfBirth, command.Address, command.IsMale, cancellationToken);
    }

    #endregion
}