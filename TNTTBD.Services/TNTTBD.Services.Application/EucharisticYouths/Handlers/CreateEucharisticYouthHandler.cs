using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Create Eucharistic Youth handler
/// </summary>
public class CreateEucharisticYouthHandler : IRequestHandler<CreateEucharisticYouthCommand, Guid>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IUnitOfWork  _unitOfWork;

    #endregion

    #region Constructors

    public CreateEucharisticYouthHandler(
        IEucharisticYouthRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(
        CreateEucharisticYouthCommand command,
        CancellationToken cancellationToken)
    {
        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        EucharisticYouth youth = new EucharisticYouth(
            Guid.NewGuid(),
            fullName,
            command.DateOfBirth,
            command.Address,
            command.IsMale
        );

        await _repository.AddAsync(youth, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return youth.Id;
    }

    #endregion
}