using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Common;

/// <summary>
/// A milestone godparent (sponsor) is a contact; it is also added to the youth's contacts as <see cref="ContactRelationType.Godparent"/>.
/// </summary>
public static class GodparentLinker
{
    /// <summary>
    /// Fail before persisting anything when a sponsor contact does not exist
    /// </summary>
    public static async Task EnsureSponsorsExistAsync(this IContactRepository contacts, IEnumerable<Guid?> sponsorContactIds, CancellationToken cancellationToken)
    {
        foreach (Guid id in sponsorContactIds.Where(i => i.HasValue && i.Value != Guid.Empty).Select(i => i!.Value).Distinct())
        {
            if (await contacts.GetByIdAsync(id, cancellationToken) == null)
            {
                throw new DomainException(DomainErrors.ValidationNotFound);
            }
        }
    }

    /// <summary>
    /// Link the sponsor to the youth unless the contact is already linked (with any relation).
    /// <paramref name="linkedInThisRequest"/> holds contacts linked earlier in the same unit of work (not yet saved).
    /// </summary>
    public static async Task LinkGodparentAsync(
        this IEucharisticYouthContactRepository links,
        Guid youthId,
        MilestoneType type,
        Guid? sponsorContactId,
        ISet<Guid> linkedInThisRequest,
        CancellationToken cancellationToken)
    {
        if (!SpiritualMilestone.SupportsSponsor(type) || sponsorContactId is not { } contactId || contactId == Guid.Empty)
        {
            return;
        }
        if (linkedInThisRequest.Contains(contactId) || await links.ExistsAsync(youthId, contactId, cancellationToken))
        {
            return;
        }
        await links.AddAsync(new EucharisticYouthContact(Guid.NewGuid(), youthId, contactId, ContactRelationType.Godparent), cancellationToken);
        linkedInThisRequest.Add(contactId);
    }
}
