using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Contacts.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class UnlinkContactFromYouthHandler : IRequestHandler<UnlinkContactFromYouthCommand, bool>
{
    private readonly IAuthorizationContext _authContext;
    private readonly IEucharisticYouthContactRepository _youthContactRepository;

    public UnlinkContactFromYouthHandler(
        IAuthorizationContext authContext,
        IEucharisticYouthContactRepository youthContactRepository)
    {
        _authContext = authContext;
        _youthContactRepository = youthContactRepository;
    }

    public async Task<bool> Handle(UnlinkContactFromYouthCommand command, CancellationToken cancellationToken)
    {
        EucharisticYouthContact? link = await _youthContactRepository.GetByIdAsync(command.Id, cancellationToken);
        if (link == null)
        {
            return false;
        }

        bool allowed = await _authContext.CanAccessYouthAsync(link.EucharisticYouthId, cancellationToken)
            || await _authContext.GetCurrentContactIdAsync(cancellationToken) == link.ContactId;
        if (!allowed)
        {
            return false;
        }

        await _youthContactRepository.RemoveAsync(link.Id, cancellationToken);

        return true;
    }
}
