using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MilestoneType Type { get; set; }
    public DateOnly Date { get; set; }
    public string? Place { get; set; }
    public int ParticipantCount { get; set; }
}

public class MilestoneEventDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MilestoneType Type { get; set; }
    public DateOnly Date { get; set; }
    public string? Place { get; set; }
    public string? Celebrant { get; set; }
    public string? Note { get; set; }
    public string? CodeTemplate { get; set; }
    public int StartIndex { get; set; }
    public List<MilestoneEventParticipantDto> Participants { get; set; } = [];
}

public class MilestoneEventParticipantDto
{
    public Guid MilestoneId { get; set; }
    public Guid EucharisticYouthId { get; set; }
    public string YouthCode { get; set; } = string.Empty;
    public FullName FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public string? ClassName { get; set; }
    public DateOnly? Date { get; set; }
    public string? Place { get; set; }
    public string? Celebrant { get; set; }
    public string? DocumentCode { get; set; }
    public MilestoneSponsorDto? Sponsor { get; set; }
    public string? Note { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public List<MilestonePrerequisiteWarningDto> Warnings { get; set; } = [];
}

/// <summary>
/// Godparent contact
/// </summary>
public class MilestoneSponsorDto
{
    public Guid ContactId { get; set; }
    public FullName FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}

public class MilestonePrerequisiteWarningDto
{
    public MilestoneType Type { get; set; }
    public MilestonePrerequisiteIssue Issue { get; set; }
}

public class MilestoneEventCandidateDto
{
    public Guid EucharisticYouthId { get; set; }
    public string YouthCode { get; set; } = string.Empty;
    public FullName FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public string? ClassName { get; set; }
    public bool HasBaptism { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public MilestoneEventCandidateMilestoneDto? ExistingMilestone { get; set; }
    public List<MilestonePrerequisiteWarningDto> Warnings { get; set; } = [];
}

public class MilestoneEventCandidateMilestoneDto
{
    public Guid MilestoneId { get; set; }
    public DateOnly? Date { get; set; }
    public string? Place { get; set; }
    public string? Celebrant { get; set; }
    public string? DocumentCode { get; set; }
    public MilestoneSponsorDto? Sponsor { get; set; }
    public string? Note { get; set; }
    public Guid? MilestoneEventId { get; set; }
    public string? MilestoneEventName { get; set; }
}

public class SaveMilestoneEventParticipantDto
{
    public Guid EucharisticYouthId { get; set; }
    public Guid? MilestoneId { get; set; }
    public DateOnly? Date { get; set; }
    public string? Place { get; set; }
    public string? Celebrant { get; set; }
    public string? DocumentCode { get; set; }
    public Guid? SponsorContactId { get; set; }
    public string? Note { get; set; }
}

public class GenerateMilestoneCodesResultDto
{
    public List<string> Codes { get; set; } = [];
}

public class MilestoneEventExportDto
{
    public string Name { get; set; } = string.Empty;
    public MilestoneType Type { get; set; }
    public DateOnly Date { get; set; }
    public List<MilestoneEventExportRowDto> Rows { get; set; } = [];
}

public class MilestoneEventExportRowDto
{
    public string? DocumentCode { get; set; }
    public FullName FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public string? Address { get; set; }
    public List<MilestoneEventExportParentDto> Parents { get; set; } = [];
    public FullName? SponsorFullName { get; set; }
    public string? Celebrant { get; set; }
}

public class MilestoneEventExportParentDto
{
    public ContactRelationType Relation { get; set; }
    public FullName FullName { get; set; } = null!;
}

public class MilestoneEventExcelFileDto
{
    public byte[] Content { get; set; } = [];
    public string FileName { get; set; } = string.Empty;
}
