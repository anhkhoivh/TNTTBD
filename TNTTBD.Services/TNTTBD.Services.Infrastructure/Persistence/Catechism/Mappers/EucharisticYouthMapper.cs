using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class EucharisticYouthMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static EucharisticYouth ToDomain(this EucharisticYouthEntity entity)
    {
        IEnumerable<SpiritualMilestone> milestones = entity.SpiritualMilestones.Select(m => m.ToDomain());
        IEnumerable<PhotoReference> photos = entity.Photos.Select(m => m.ToDomain());
        EucharisticYouth domain = new EucharisticYouth(entity.Id, entity.Code, entity.FullName, entity.DateOfBirth, entity.Address, entity.IsMale, photos, milestones, entity.PatronSaints);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static EucharisticYouthEntity ToEntity(this EucharisticYouth domain)
    {
        EucharisticYouthEntity entity = new EucharisticYouthEntity
        {
            Id = domain.Id,
            Code = domain.Code,
            FullName = domain.FullName,
            Address = domain.Address,
            DateOfBirth = domain.DateOfBirth,
            IsMale = domain.IsMale,
            PhoneNumber = domain.PhoneNumber,
            Photos = domain.Photos.Select(p => p.ToEucharisticYouthPhotoEntity(domain.Id)).ToList(),
            SpiritualMilestones = domain.Milestones.Select(m => m.ToEntity(domain.Id)).ToList(),
            PatronSaints = domain.PatronSaints.ToList()
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this EucharisticYouth domain, EucharisticYouthEntity entity)
    {
        entity.FullName = domain.FullName;
        entity.DateOfBirth = domain.DateOfBirth;
        entity.Address = domain.Address;
        entity.IsMale = domain.IsMale;
        entity.PhoneNumber = domain.PhoneNumber;

        // Photo
        foreach (PhotoReference photo in domain.Photos)
        {
            EucharisticYouthPhotoEntity? existingEntity = entity.Photos
                .FirstOrDefault(x => x.Id == photo.Id);

            if (existingEntity == null)
            {
                entity.Photos.Add(photo.ToEucharisticYouthPhotoEntity(entity.Id));
            }
            else
            {
                photo.UpdateEntity(existingEntity);
            }
        }

        // REMOVE
        List<EucharisticYouthPhotoEntity> removedPhotoList = entity.Photos
            .Where(x => domain.Photos.All(m => m.Id != x.Id))
            .ToList();

        foreach (EucharisticYouthPhotoEntity r in removedPhotoList)
        {
            entity.Photos.Remove(r);
        }

        // UPDATE + ADD milestone
        foreach (SpiritualMilestone milestone in domain.Milestones)
        {
            SpiritualMilestoneEntity? existingEntity = entity.SpiritualMilestones
                .FirstOrDefault(x => x.Id == milestone.Id);

            if (existingEntity == null)
            {
                entity.SpiritualMilestones.Add(milestone.ToEntity(entity.Id));
            }
            else
            {
                milestone.UpdateEntity(existingEntity);
            }
        }

        // REMOVE
        List<SpiritualMilestoneEntity> removedList = entity.SpiritualMilestones
            .Where(x => domain.Milestones.All(m => m.Id != x.Id))
            .ToList();

        foreach (SpiritualMilestoneEntity r in removedList)
        {
            entity.SpiritualMilestones.Remove(r);
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
    public static EucharisticYouthDto ToDTO(this EucharisticYouthEntity entity, IFileStorage storage)
    {
        return new EucharisticYouthDto
        {
            Id = entity.Id,
            FullName = entity.FullName,
            DateOfBirth = entity.DateOfBirth,
            IsMale = entity.IsMale,
            Address = entity.Address,
            PhoneNumber = entity.PhoneNumber,

            Photos = entity.Photos.Select(p => new PhotoReferenceDto
            {
                Purpose = p.Purpose.ToString(),
                ContentType = p.ContentType,
                UploadedBy = p.UploadedBy,
                Url = storage.GetFileUrl(p.StorageKey)
            }).ToList(),

            Milestones = entity.SpiritualMilestones.Select(m => new SpiritualMilestoneDto
            {
                Type = m.Type.ToString(),
                Date = m.Date,
                Place = m.Place,
                Note = m.Note
            }).ToList(),

            PatronSaints = entity.PatronSaints.Select(p => new PatronSaintDto
            {
                SaintName = p.SaintName,
                Day = p.Day,
                Month = p.Month
            }).ToList()
        };
    }

    #endregion
}