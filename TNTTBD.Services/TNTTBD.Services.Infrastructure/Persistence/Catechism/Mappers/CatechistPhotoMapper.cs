using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechistPhotoMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static PhotoReference ToDomain(this CatechistPhotoEntity entity)
    {
        PhotoReference domain = new(entity.Id, entity.StorageKey, entity.ContentType, entity.Purpose, entity.UploadedBy, entity.UploadedAt, entity.Visibility);
        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechistPhotoEntity ToCatechistPhotoEntity(this PhotoReference domain, Guid catechistId)
    {
        CatechistPhotoEntity entity = new CatechistPhotoEntity
        {
            Id = domain.Id,
            CatechistId = catechistId,
            StorageKey = domain.StorageKey,
            ContentType = domain.ContentType,
            Purpose = domain.Purpose,
            UploadedAt = domain.UploadedAt,
            UploadedBy = domain.UploadedBy,
            Visibility = domain.Visibility
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this PhotoReference domain, CatechistPhotoEntity entity)
    {
        entity.StorageKey = domain.StorageKey;
        entity.ContentType = domain.ContentType;
        entity.Purpose = domain.Purpose;
        entity.UploadedAt = domain.UploadedAt;
        entity.UploadedBy = domain.UploadedBy;
        entity.Visibility = domain.Visibility;
    }

    #endregion
}