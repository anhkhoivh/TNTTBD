namespace TNTTBD.Services.Domain.Common;

public static class DomainErrors
{
    public const string AttendanceAlreadyRecorded = "attendance.already_record";
    public const string MilestoneAlreadyExists = "milestone.already_exists";
    public const string NotBaptised = "milestone.not_baptised";
    public const string PatronSaintAlreadyAssigned = "patron_saint.already_assigned";
    public const string ValidationRequired = "validation.required";
    public const string ValidationNotFound = "validation.not_found";
    public const string ValidationAlreadyExists = "validation.already_exists";
}