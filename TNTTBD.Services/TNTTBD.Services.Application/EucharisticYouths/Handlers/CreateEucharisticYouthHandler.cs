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
    private readonly IEucharisticYouthRunningNumberRepository _runningNumberRepository;

    #endregion

    #region Constructors

    public CreateEucharisticYouthHandler(
        IEucharisticYouthRepository repository,
        IEucharisticYouthRunningNumberRepository runningNumberRepository)
    {
        _repository = repository;
        _runningNumberRepository = runningNumberRepository;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(CreateEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        int year = DateTimeOffset.Now.Year;
        int currentNumber = await _runningNumberRepository.IncreaseRunningNumber(year, cancellationToken);

        string code = $"{year}{currentNumber:D3}";

        EucharisticYouth youth = new EucharisticYouth(
            Guid.NewGuid(),
            code,
            fullName,
            command.DateOfBirth,
            command.Address,
            command.IsMale
        );

        await _repository.AddAsync(youth, cancellationToken);

        return youth.Id;
    }

    #endregion
}