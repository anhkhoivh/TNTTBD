using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechistMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static Catechist ToDomain(this CatechistEntity entity)
    {
        IEnumerable<PhotoReference> photos = entity.Photos.Select(m => m.ToDomain());
        Catechist domain = new Catechist(entity.Id, entity.FullName, entity.DateOfBirth, entity.Address, entity.IsMale, photos, entity.PatronSaints);
        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechistEntity ToEntity(this Catechist domain)
    {
        CatechistEntity entity = new CatechistEntity
        {
            Id = domain.Id,
            FullName = domain.FullName,
            Address = domain.Address,
            DateOfBirth = domain.DateOfBirth,
            IsMale = domain.IsMale,
            Photos = domain.Photos.Select(p => p.ToCatechistPhotoEntity(domain.Id)).ToList(),
            PatronSaints = domain.PatronSaints.ToList()
        };
        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this Catechist domain, CatechistEntity entity)
    {
        entity.FullName = domain.FullName;
        entity.DateOfBirth = domain.DateOfBirth;
        entity.Address = domain.Address;
        entity.IsMale = domain.IsMale;

        // Photo
        foreach (PhotoReference photo in domain.Photos)
        {
            CatechistPhotoEntity? existingEntity = entity.Photos.FirstOrDefault(x => x.Id == photo.Id);

            if (existingEntity == null)
            {
                entity.Photos.Add(photo.ToCatechistPhotoEntity(entity.Id));
            }
            else
            {
                photo.UpdateEntity(existingEntity);
            }
        }

        // REMOVE
        List<CatechistPhotoEntity> removedPhotoList = entity.Photos
            .Where(x => domain.Photos.All(m => m.Id != x.Id))
            .ToList();

        foreach (CatechistPhotoEntity r in removedPhotoList)
        {
            entity.Photos.Remove(r);
        }

        entity.PatronSaints.Clear();
        foreach (PatronSaint patronSaint in domain.PatronSaints)
        {
            entity.PatronSaints.Add(patronSaint);
        }
    }

    #endregion

    #region Convert methods from entity to DTO

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static CatechistDto ToDTO(this CatechistEntity entity, IFileStorage storage)
    {
        return new CatechistDto
        {
            Id = entity.Id,
            ChristName = entity.FullName.ChristName,
            GivenName = entity.FullName.GivenName,
            FamilyName = entity.FullName.FamilyName,
            MiddleName = entity.FullName.MiddleName,
            DateOfBirth = entity.DateOfBirth,
            IsMale = entity.IsMale,
            Address = entity.Address,

            Photos = entity.Photos.Select(m => m.ToDTO(storage)).ToList(),

            PatronSaints = entity.PatronSaints
                .Select(p => new PatronSaintDto
                {
                    SaintName = p.SaintName,
                    Day = p.Day,
                    Month = p.Month
                })
                .ToList()
        };
    }

    #endregion
}