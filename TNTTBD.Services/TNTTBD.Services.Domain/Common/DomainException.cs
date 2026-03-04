namespace TNTTBD.Services.Domain.Common;

/// <summary>
/// Handle exception
/// </summary>
public class DomainException : Exception
{
    public DomainException(string errorCode) : base()
    {
        ErrorCode = errorCode;
    }

    #region Properties

    public string ErrorCode { get; }

    #endregion
}