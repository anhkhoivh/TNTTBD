using MediatR;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;
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
    private readonly IContactRepository _contactRepository;
    private readonly IEucharisticYouthContactRepository _youthContactRepository;
    private readonly ICatechismClassEucharisticYouthRepository _catechismClassEucharisticYouthRepository;

    #endregion

    #region Constructors

    public CreateEucharisticYouthHandler(
        IEucharisticYouthRepository repository,
        IEucharisticYouthRunningNumberRepository runningNumberRepository,
        IContactRepository contactRepository,
        IEucharisticYouthContactRepository youthContactRepository,
        ICatechismClassEucharisticYouthRepository catechismClassEucharisticYouthRepository)
    {
        _repository = repository;
        _runningNumberRepository = runningNumberRepository;
        _contactRepository = contactRepository;
        _youthContactRepository = youthContactRepository;
        _catechismClassEucharisticYouthRepository = catechismClassEucharisticYouthRepository;
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

        // Add patron saints
        if (command.PatronSaints != null)
        {
            foreach (var ps in command.PatronSaints)
            {
                youth.AddPatronSaint(new PatronSaint(ps.SaintName, ps.Day, ps.Month));
            }
        }

        // Add milestones
        if (command.Milestones != null)
        {
            foreach (var m in command.Milestones)
            {
                if (m.Date is null) continue;
                youth.AddMilestone(new SpiritualMilestone(
                    Guid.NewGuid(),
                    m.Type,
                    m.Date.Value,
                    m.DocumentCode,
                    m.Place,
                    m.Note));
            }
        }

        await _repository.AddAsync(youth, cancellationToken);

        // Create contacts and link to youth
        if (command.Contacts != null)
        {
            foreach (var c in command.Contacts)
            {
                var contactName = new FullName(c.GivenName, c.ChristName, c.FamilyName, c.MiddleName);
                var contact = new Contact(Guid.NewGuid(), contactName, c.PhoneNumber, c.Email);
                await _contactRepository.AddAsync(contact, cancellationToken);

                var link = new EucharisticYouthContact(
                    Guid.NewGuid(),
                    youth.Id,
                    contact.Id,
                    c.RelationType);
                await _youthContactRepository.AddAsync(link, cancellationToken);
            }
        }

        if (command.CatechismClassId != null)
        {
            CatechismClassEucharisticYouth domain = new(Guid.NewGuid(), youth.Id, command.CatechismClassId.Value, DateTimeOffset.Now.ToUniversalTime(), null, EnrollmentStatus.Active, null);
            await _catechismClassEucharisticYouthRepository.AddAsync(domain, cancellationToken);
        }

        return youth.Id;
    }

    #endregion
}