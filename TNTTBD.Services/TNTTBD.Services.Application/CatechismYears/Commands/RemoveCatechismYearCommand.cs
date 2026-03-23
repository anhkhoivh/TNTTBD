using MediatR;

namespace TNTTBD.Services.Application.CatechismYears.Commands;

public record RemoveCatechismYearCommand(
    Guid CatechismYearId
) : IRequest;