using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Auth.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public bool IsActive { get; set; }
    public UserRole UserRole { get; set; }
    public Guid? CatechistId { get; set; }
    public Guid? EucharisticYouthId { get; set; }
    public Guid? ContactId { get; set; }
}
