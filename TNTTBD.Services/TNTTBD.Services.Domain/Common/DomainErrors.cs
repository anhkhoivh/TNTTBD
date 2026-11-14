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
    public const string ContactAlreadyLinked = "contact.already_linked";
    public const string EmailAlreadyExists = "user.email_already_exists";
    public const string MainCatechistAlreadyActive = "class_catechist.main_already_active";
    public const string ExamScoreOutOfRange = "exam.score_out_of_range";
    public const string ExamRemarkTooLong = "exam.remark_too_long";
    public const string MilestoneLinkedToEvent = "milestone.linked_to_event";
    public const string MilestoneEventNotEmpty = "milestone_event.not_empty";
    public const string MilestoneEventTypeLocked = "milestone_event.type_locked";
    public const string MilestoneEventConflict = "milestone_event.conflict";
    public const string MilestoneEventInvalidTemplate = "milestone_event.invalid_template";
    public const string MilestoneEventDuplicateYouth = "milestone_event.duplicate_youth";
    public const string MilestoneEventUnlinkedExisting = "milestone_event.unlinked_existing";
}