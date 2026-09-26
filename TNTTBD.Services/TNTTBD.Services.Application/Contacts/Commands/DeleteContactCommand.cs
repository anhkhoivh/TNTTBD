using MediatR;

namespace TNTTBD.Services.Application.Contacts.Commands;

public record DeleteContactCommand(Guid Id) : IRequest<bool>;
