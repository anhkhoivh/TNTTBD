using MediatR;
using TNTTBD.Services.Application.Contacts.DTOs;

namespace TNTTBD.Services.Application.Contacts.Queries;

public record GetContactByIdQuery(Guid Id) : IRequest<ContactDetailDto?>;
