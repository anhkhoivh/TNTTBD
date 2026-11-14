namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

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
