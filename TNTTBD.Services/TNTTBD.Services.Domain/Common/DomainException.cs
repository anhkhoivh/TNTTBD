namespace TNTTBD.Services.Domain.Common;

/// <summary>
/// Handle exception
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}