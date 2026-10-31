using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class CreateCatechistHandler : IRequestHandler<CreateCatechistCommand, Guid?>
{
    #region Fields

    private readonly ICatechistRepository _repository;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public CreateCatechistHandler(ICatechistRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<Guid?> Handle(CreateCatechistCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return null;
        }

        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);

        Catechist catechist = new (
            Guid.NewGuid(),
            fullName,
            command.DateOfBirth,
            command.Address,
            command.IsMale,
            command.PhoneNumber
        );

        // Add patron saints
        if (command.PatronSaints != null)
        {
            foreach (var ps in command.PatronSaints)
            {
                catechist.AddPatronSaint(new PatronSaint(ps.SaintName, ps.Day, ps.Month));
            }
        }

        await _repository.AddAsync(catechist, cancellationToken);

        return catechist.Id;
    }

    #endregion
}