using MediatR;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Queries;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

public class GetCatechismClassByIdHandler : IRequestHandler<GetCatechismClassByIdQuery, CatechismClassDto?>
{
    #region Fields

    private readonly ICatechismClassRepository _repository;

    #endregion

    #region Constructors

    public GetCatechismClassByIdHandler(ICatechismClassRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<CatechismClassDto?> Handle(GetCatechismClassByIdQuery query, CancellationToken cancellationToken)
    {
        CatechismClass? catechismClass = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (catechismClass == null)
        {
            return null;
        }

        return new CatechismClassDto
        {
            Id = catechismClass.Id,
            ClassName = catechismClass.ClassName
        };
    }

    #endregion
}