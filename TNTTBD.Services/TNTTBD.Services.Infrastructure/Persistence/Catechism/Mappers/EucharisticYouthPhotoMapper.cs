using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class EucharisticYouthPhotoMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static PhotoReference ToDomain(this EucharisticYouthPhotoEntity entity)
    {
        PhotoReference domain = new(entity.Id, entity.StorageKey, entity.ContentType, entity.Purpose, entity.UploadedBy, entity.UploadedAt);
        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static EucharisticYouthPhotoEntity ToEucharisticYouthPhotoEntity(this PhotoReference domain, Guid eucharisticYouthId)
    {
        EucharisticYouthPhotoEntity entity = new EucharisticYouthPhotoEntity
        {
            Id = domain.Id,
            EucharisticYouthId = eucharisticYouthId,
            StorageKey = domain.StorageKey,
            ContentType = domain.ContentType,
            Purpose = domain.Purpose,
            UploadedAt = domain.UploadedAt,
            UploadedBy = domain.UploadedBy
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this PhotoReference domain, EucharisticYouthPhotoEntity entity)
    {
        entity.StorageKey = domain.StorageKey;
        entity.ContentType = domain.ContentType;
        entity.Purpose = domain.Purpose;
        entity.UploadedAt = domain.UploadedAt;
        entity.UploadedBy = domain.UploadedBy;
    }

    #endregion

    #region Convert methods from entity to DTO

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static PhotoReferenceDto ToDTO(this EucharisticYouthPhotoEntity entity, IFileStorage storage)
    {
        return new PhotoReferenceDto
            {
                PhotoId = entity.Id,
                Purpose = entity.Purpose.ToString(),
                ContentType = entity.ContentType,
                UploadedBy = entity.UploadedBy,
                Url = storage.GetFileUrl(entity.StorageKey)
            };
    }

    #endregion
}