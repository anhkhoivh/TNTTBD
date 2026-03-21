using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistByIdHandler : IRequestHandler<GetCatechistByIdQuery, CatechistDto?>
{
    #region Fields

    private readonly ICatechistRepository _repository;
    private readonly IFileStorage _storage;

    #endregion

    #region Constructors

    public GetCatechistByIdHandler(ICatechistRepository repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _storage = fileStorage;
    }

    #endregion

    #region Methods

    public async Task<CatechistDto?> Handle(GetCatechistByIdQuery query, CancellationToken cancellationToken)
    {
        Catechist? catechist = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (catechist == null)
        {
            return null;
        }

        return new CatechistDto
        {
            Id = catechist.Id,
            ChristName = catechist.FullName.ChristName,
            GivenName = catechist.FullName.GivenName,
            FamilyName = catechist.FullName.FamilyName,
            MiddleName = catechist.FullName.MiddleName,
            DateOfBirth = catechist.DateOfBirth,
            IsMale = catechist.IsMale,
            Address = catechist.Address,

            Photos = catechist.Photos
                .Select(m => new PhotoReferenceDto
                {
                    Purpose = m.Purpose.ToString(),
                    ContentType = m.ContentType,
                    UploadedBy = m.UploadedBy,
                    Url = _storage.GetFileUrl(m.StorageKey)
                })
                .ToList(),

            PatronSaints = catechist.PatronSaints
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