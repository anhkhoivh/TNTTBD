using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class UserExternalLoginMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static UserExternalLogin ToDomain(this UserExternalLoginEntity entity)
    {
        UserExternalLogin domain = new UserExternalLogin(entity.Id, entity.UserId, entity.Provider, entity.SubjectId, entity.LinkedAt);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static UserExternalLoginEntity ToEntity(this UserExternalLogin domain)
    {
        UserExternalLoginEntity entity = new UserExternalLoginEntity
        {
            Id = domain.Id,
            UserId = domain.UserId,
            SubjectId = domain.SubjectId,
            Provider = domain.Provider,
            LinkedAt = domain.LinkedAt
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this UserExternalLogin domain, UserExternalLoginEntity entity)
    {
        entity.UserId = domain.UserId;
        entity.SubjectId = domain.SubjectId;
        entity.Provider = domain.Provider;
        entity.LinkedAt = domain.LinkedAt;
    }

    #endregion
}