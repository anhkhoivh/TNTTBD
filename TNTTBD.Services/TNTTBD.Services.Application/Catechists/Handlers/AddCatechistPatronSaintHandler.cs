using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddCatechistPatronSaintHandler : IRequestHandler<AddCatechistPatronSaintCommand>
{
    #region Fields

    private readonly ICatechistRepository _repository;

    #endregion

    #region Constructors

    public AddCatechistPatronSaintHandler(ICatechistRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(AddCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        PatronSaint patronSaint = new(command.SaintName, command.Day, command.Month);
        await _repository.AddPatronSaint(command.CatechistId, patronSaint, cancellationToken);
    }

    #endregion
}