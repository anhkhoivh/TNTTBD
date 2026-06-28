using MediatR;
using TNTTBD.Services.Application.Registration.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Registration.Handlers;

public class RegisterEucharisticYouthHandler : IRequestHandler<RegisterEucharisticYouthCommand, Guid>
{
    private readonly IEucharisticYouthRepository _youthRepository;
    private readonly IEucharisticYouthRunningNumberRepository _runningNumberRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IEucharisticYouthContactRepository _youthContactRepository;

    public RegisterEucharisticYouthHandler(
        IEucharisticYouthRepository youthRepository,
        IEucharisticYouthRunningNumberRepository runningNumberRepository,
        IContactRepository contactRepository,
        IEucharisticYouthContactRepository youthContactRepository)
    {
        _youthRepository = youthRepository;
        _runningNumberRepository = runningNumberRepository;
        _contactRepository = contactRepository;
        _youthContactRepository = youthContactRepository;
    }

    public async Task<Guid> Handle(RegisterEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        // 1. Create EucharisticYouth
        var fullName = new FullName(
            command.Youth.FullName.GivenName,
            command.Youth.FullName.ChristName,
            command.Youth.FullName.FamilyName,
            command.Youth.FullName.MiddleName);

        int year = DateTimeOffset.UtcNow.Year;
        int currentNumber = await _runningNumberRepository.IncreaseRunningNumber(year, cancellationToken);
        string code = $"{year}{currentNumber:D3}";

        var youth = new EucharisticYouth(
            Guid.NewGuid(),
            code,
            fullName,
            command.Youth.DateOfBirth,
            command.Youth.Address,
            command.Youth.IsMale);

        // 2. Add Baptism milestone if provided
        if (command.Youth.Baptism is not null)
        {
            var milestone = new SpiritualMilestone(
                Guid.NewGuid(),
                MilestoneType.Baptism,
                command.Youth.Baptism.Date,
                null,
                command.Youth.Baptism.Place);

            youth.AddMilestone(milestone);
        }

        await _youthRepository.AddAsync(youth, cancellationToken);

        // 3. Create contacts and link to youth
        foreach (var contactData in command.Contacts)
        {
            var contactName = new FullName(
                contactData.GivenName,
                null,
                contactData.FamilyName);

            var contact = new Contact(
                Guid.NewGuid(),
                contactName,
                contactData.PhoneNumber,
                contactData.Email);

            await _contactRepository.AddAsync(contact, cancellationToken);

            var link = new EucharisticYouthContact(
                Guid.NewGuid(),
                youth.Id,
                contact.Id,
                contactData.Relation);

            await _youthContactRepository.AddAsync(link, cancellationToken);
        }

        return youth.Id;
    }
}
