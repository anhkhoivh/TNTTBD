using MediatR;
using TNTTBD.Services.Application.CatechismSessions.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Queries;

public record GetSessionsByClassQuery(Guid CatechismClassId) : IRequest<List<CatechismSessionDto>>;