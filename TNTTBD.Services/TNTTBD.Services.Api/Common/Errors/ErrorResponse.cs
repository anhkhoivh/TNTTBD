namespace TNTTBD.Services.Api.Common.Errors;

/// <summary>
/// Error response
/// </summary>
public class ErrorResponse
{
    public string Type { get; set; } = "https://httpstatuses.com/400";
    public string Title { get; set; } = "Bad Request";
    public int Status { get; set; }
    public required string ErrorCode { get; set; }
    public required string Message { get; set; }
    public required string TraceId { get; set; }
    public object? Details { get; set; }
}