using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Domain.Common;
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
public class CreateEucharisticYouthHandler : IRequestHandler<CreateEucharisticYouthCommand, Guid?>
{
    #region Fields

    private readonly IAuthorizationContext _authContext;
    private readonly IEucharisticYouthRepository _repository;
    private readonly IEucharisticYouthRunningNumberRepository _runningNumberRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IEucharisticYouthContactRepository _youthContactRepository;
    private readonly ICatechismClassEucharisticYouthRepository _catechismClassEucharisticYouthRepository;

    #endregion

    #region Constructors

    public CreateEucharisticYouthHandler(
        IAuthorizationContext authContext,
        IEucharisticYouthRepository repository,
        IEucharisticYouthRunningNumberRepository runningNumberRepository,
        IContactRepository contactRepository,
        IEucharisticYouthContactRepository youthContactRepository,
        ICatechismClassEucharisticYouthRepository catechismClassEucharisticYouthRepository)
    {
        _authContext = authContext;
        _repository = repository;
        _runningNumberRepository = runningNumberRepository;
        _contactRepository = contactRepository;
        _youthContactRepository = youthContactRepository;
        _catechismClassEucharisticYouthRepository = catechismClassEucharisticYouthRepository;
    }

    #endregion

    #region Methods

    public async Task<Guid?> Handle(CreateEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminOrCatechistAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }

        if (command.CatechismClassId != null &&
            !await _authContext.CanAccessClassAsync(command.CatechismClassId.Value, cancellationToken))
        {
            return null;
        }

        // Resolve existing contacts up front so a bad id fails before anything is persisted.
        Dictionary<Guid, Contact> existingContacts = new();
        foreach (Guid contactId in (command.Contacts ?? []).Where(c => c.ContactId != null).Select(c => c.ContactId!.Value).Distinct())
        {
            Contact existing = await _contactRepository.GetByIdAsync(contactId, cancellationToken)
                ?? throw new DomainException(DomainErrors.ValidationNotFound);
            existingContacts[contactId] = existing;
        }
        await _contactRepository.EnsureSponsorsExistAsync((command.Milestones ?? []).Select(m => m.SponsorContactId), cancellationToken);

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
                    m.Note,
                    m.SponsorContactId,
                    m.Celebrant));
            }
        }

        await _repository.AddAsync(youth, cancellationToken);

        // Link existing contacts or create new ones, then link to youth
        HashSet<Guid> linkedContactIds = new();
        if (command.Contacts != null)
        {
            foreach (var c in command.Contacts)
            {
                Guid contactId;
                if (c.ContactId != null)
                {
                    contactId = existingContacts[c.ContactId.Value].Id;
                    if (!linkedContactIds.Add(contactId)) continue;
                }
                else
                {
                    var contactName = new FullName(c.GivenName ?? string.Empty, c.ChristName, c.FamilyName, c.MiddleName);
                    var contact = new Contact(Guid.NewGuid(), contactName, c.PhoneNumber, c.Email);
                    await _contactRepository.AddAsync(contact, cancellationToken);
                    contactId = contact.Id;
                }

                var link = new EucharisticYouthContact(
                    Guid.NewGuid(),
                    youth.Id,
                    contactId,
                    c.RelationType);
                await _youthContactRepository.AddAsync(link, cancellationToken);
            }
        }

        foreach (SpiritualMilestone m in youth.Milestones)
        {
            await _youthContactRepository.LinkGodparentAsync(youth.Id, m.Type, m.SponsorContactId, linkedContactIds, cancellationToken);
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