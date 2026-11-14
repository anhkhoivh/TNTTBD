using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism;

public enum MilestonePrerequisiteIssue
{
    /// <summary>The prerequisite milestone is not recorded</summary>
    Missing,
    /// <summary>The prerequisite milestone is dated after this milestone</summary>
    DatedAfter,
}

public record MilestonePrerequisiteWarning(MilestoneType Type, MilestonePrerequisiteIssue Issue);

/// <summary>
/// Usual sacramental order. Used for non-blocking warnings only (records may simply be missing).
/// </summary>
public static class MilestonePrerequisites
{
    private static readonly Dictionary<MilestoneType, MilestoneType[]> Rules = new()
    {
        [MilestoneType.FirstPenance] = [MilestoneType.Baptism],
        [MilestoneType.FirstCommunion] = [MilestoneType.Baptism, MilestoneType.FirstPenance],
        [MilestoneType.Confirmation] = [MilestoneType.Baptism, MilestoneType.FirstCommunion],
        [MilestoneType.SolemnCommunion] = [MilestoneType.Baptism, MilestoneType.FirstCommunion],
    };

    public static IReadOnlyList<MilestoneType> For(MilestoneType type) =>
        Rules.TryGetValue(type, out MilestoneType[]? required) ? required : [];

    /// <summary>
    /// Check a youth's recorded milestones against the prerequisites of <paramref name="type"/> received on <paramref name="date"/>.
    /// </summary>
    public static List<MilestonePrerequisiteWarning> Check(
        MilestoneType type,
        DateOnly? date,
        IEnumerable<(MilestoneType Type, DateOnly? Date)> recorded)
    {
        List<(MilestoneType Type, DateOnly? Date)> list = recorded.ToList();
        List<MilestonePrerequisiteWarning> warnings = [];
        foreach (MilestoneType required in For(type))
        {
            var match = list.FirstOrDefault(m => m.Type == required);
            if (list.All(m => m.Type != required))
            {
                warnings.Add(new MilestonePrerequisiteWarning(required, MilestonePrerequisiteIssue.Missing));
            }
            else if (date.HasValue && match.Date.HasValue && match.Date.Value > date.Value)
            {
                warnings.Add(new MilestonePrerequisiteWarning(required, MilestonePrerequisiteIssue.DatedAfter));
            }
        }
        return warnings;
    }
}
