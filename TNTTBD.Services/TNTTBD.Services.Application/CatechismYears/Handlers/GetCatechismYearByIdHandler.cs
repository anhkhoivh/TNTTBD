using MediatR;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.CatechismYears.Queries;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismYears.Handlers;

public class GetCatechismYearByIdHandler : IRequestHandler<GetCatechismYearByIdQuery, CatechismYearDto?>
{
    #region Fields

    private readonly ICatechismYearRepository _repository;

    #endregion

    #region Constructors

    public GetCatechismYearByIdHandler(ICatechismYearRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<CatechismYearDto?> Handle(GetCatechismYearByIdQuery query, CancellationToken cancellationToken)
    {
        CatechismYear? year = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (year == null)
        {
            return null;
        }

        return new CatechismYearDto
        {
            Id = year.Id,
            StartDate = year.StartDate,
            EndDate = year.EndDate,
            Theme = year.Theme
        };
    }

    #endregion
}