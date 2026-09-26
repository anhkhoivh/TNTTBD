using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class ContactMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static Contact ToDomain(this ContactEntity entity)
    {
        Contact domain = new Contact(entity.Id, entity.FullName, entity.PhoneNumber, entity.Email);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static ContactEntity ToEntity(this Contact domain)
    {
        ContactEntity entity = new ContactEntity
        {
            Id = domain.Id,
            FullName = domain.FullName,
            PhoneNumber = domain.PhoneNumber,
            Email = domain.Email,
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this Contact domain, ContactEntity entity)
    {
        entity.FullName = domain.FullName;
        entity.PhoneNumber = domain.PhoneNumber;
        entity.Email = domain.Email;
    }

    #endregion
}