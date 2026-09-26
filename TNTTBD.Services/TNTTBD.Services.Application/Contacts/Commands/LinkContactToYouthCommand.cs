using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Contacts.Commands;

public record LinkContactToYouthCommand(
    Guid EucharisticYouthId,
    Guid ContactId,
    ContactRelationType RelationType
) : IRequest<Guid?>;
