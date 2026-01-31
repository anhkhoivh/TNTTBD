using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class UserMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static User ToDomain(this UserEntity entity)
    {
        User user = new User(entity.Id, entity.Email, entity.UserName);

        if (entity.IsActive == false)
        {
            user.Deactivate();
        }

        if (entity.CatechistId.HasValue)
        {
            user.LinkCatechist(entity.CatechistId.Value);
        }

        if (entity.ContactId.HasValue)
        {
            user.LinkContact(entity.ContactId.Value);
        }

        if (entity.EucharisticYouthId.HasValue)
        {
            user.LinkEucharisticYouth(entity.EucharisticYouthId.Value);
        }

        foreach (var login in entity.ExternalLogins)
        {
            user.AddExternalLogin(login.Provider, login.SubjectId);
        }

        return user;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static UserEntity ToEntity(this User domain)
    {
        var entity = new UserEntity
        {
            Id = domain.Id,
            Email = domain.Email,
            UserName = domain.UserName,
            IsActive = domain.IsActive,
            CatechistId = domain.CatechistId,
            ContactId = domain.ContactId,
            EucharisticYouthId = domain.EucharisticYouthId
        };

        foreach (var login in domain.ExternalLogins)
        {
            entity.ExternalLogins.Add(new UserExternalLoginEntity
            {
                Id = login.Id,
                UserId = domain.Id,
                Provider = login.Provider,
                SubjectId = login.SubjectId,
                LinkedAt = login.LinkedAt
            });
        }

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(
    this User domain,
    UserEntity entity)
{
    entity.Email = domain.Email;
    entity.UserName = domain.UserName;
    entity.IsActive = domain.IsActive;

    entity.CatechistId = domain.CatechistId;
    entity.ContactId = domain.ContactId;
    entity.EucharisticYouthId = domain.EucharisticYouthId;

    // External logins: sync
    entity.ExternalLogins.Clear();

    foreach (var login in domain.ExternalLogins)
    {
        entity.ExternalLogins.Add(new UserExternalLoginEntity
        {
            Id = login.Id,
            UserId = domain.Id,
            Provider = login.Provider,
            SubjectId = login.SubjectId,
            LinkedAt = login.LinkedAt
        });
    }
}

    #endregion
}