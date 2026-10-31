using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetEucharisticYouthPhotoPermissionsQuery(Guid YouthId) : IRequest<bool>;
