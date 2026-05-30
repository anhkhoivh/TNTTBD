using MediatR;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Users.Queries;

namespace TNTTBD.Services.Application.Users.Handlers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginatedResult<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResult<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllAsync(query.Page, query.PageSize, cancellationToken);
    }
}
