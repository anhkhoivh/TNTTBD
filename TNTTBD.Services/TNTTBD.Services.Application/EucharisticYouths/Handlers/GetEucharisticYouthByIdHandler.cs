using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Queries;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthByIdHandler : IRequestHandler<GetEucharisticYouthByIdQuery, EucharisticYouthDto?>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public GetEucharisticYouthByIdHandler(
        IEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<EucharisticYouthDto?> Handle(
        GetEucharisticYouthByIdQuery query,
        CancellationToken cancellationToken)
    {
        EucharisticYouth? youth = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (youth == null)
        {
            return null;
        }

        return new EucharisticYouthDto
        {
            Id = youth.Id,
            ChristName = youth.FullName.ChristName,
            GivenName = youth.FullName.GivenName,
            FamilyName = youth.FullName.FamilyName,
            MiddleName = youth.FullName.MiddleName,
            DateOfBirth = youth.DateOfBirth,
            IsMale = youth.IsMale,
            Address = youth.Address,

            Photos = youth.Photos
                .Select(m => new PhotoReferenceDto
                {
                    Purpose = m.Purpose.ToString(),
                    ContentType = m.ContentType,
                    StorageKey = m.StorageKey,
                    Visibility = m.Visibility.ToString(),
                    UploadedAt = m.UploadedAt,
                    UploadedBy = m.UploadedBy
                })
                .ToList(),

            Milestones = youth.Milestones
                .Select(m => new SpiritualMilestoneDto
                {
                    Type = m.Type.ToString(),
                    Date = m.Date,
                    Place = m.Place,
                    Note = m.Note
                })
                .ToList(),

            PatronSaints = youth.PatronSaints
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