namespace TNTTBD.Services.Application.Auth.Commands;

public record ChangeOwnPasswordCommand(
    string CurrentPassword,
    string NewPassword
);
