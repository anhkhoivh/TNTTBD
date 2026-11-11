using System.Globalization;
using System.Text;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Controlled template for generating milestone document codes.
/// Supported tokens: {yyyy}, {yy}, {MM}, {dd}, {index}, {index:000} (zero padding, 1-9 digits).
/// Any other text is copied literally; stray braces and unknown tokens are rejected.
/// </summary>
public sealed class MilestoneCodeTemplate
{
    public const int MaxTemplateLength = 100;
    public const int MaxCodeLength = 100;

    private abstract record Part;
    private sealed record Literal(string Text) : Part;
    private sealed record DateToken(string Format) : Part;
    private sealed record IndexToken(int Padding) : Part;

    private readonly IReadOnlyList<Part> _parts;

    private MilestoneCodeTemplate(string template, IReadOnlyList<Part> parts)
    {
        Template = template;
        _parts = parts;
    }

    public string Template { get; }

    /// <summary>
    /// Parse a template or throw <see cref="DomainErrors.MilestoneEventInvalidTemplate"/>.
    /// </summary>
    public static MilestoneCodeTemplate Parse(string? template)
    {
        if (!TryParse(template, out MilestoneCodeTemplate? result))
        {
            throw new DomainException(DomainErrors.MilestoneEventInvalidTemplate);
        }
        return result!;
    }

    public static bool TryParse(string? template, out MilestoneCodeTemplate? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(template))
        {
            return false;
        }
        template = template.Trim();
        if (template.Length > MaxTemplateLength)
        {
            return false;
        }

        List<Part> parts = new();
        StringBuilder literal = new();
        int i = 0;
        while (i < template.Length)
        {
            char c = template[i];
            if (c == '}')
            {
                return false;
            }
            if (c != '{')
            {
                literal.Append(c);
                i++;
                continue;
            }

            int close = template.IndexOf('}', i + 1);
            if (close < 0)
            {
                return false;
            }
            string token = template.Substring(i + 1, close - i - 1);
            if (token.Contains('{'))
            {
                return false;
            }

            Part? part = token switch
            {
                "yyyy" => new DateToken("yyyy"),
                "yy" => new DateToken("yy"),
                "MM" => new DateToken("MM"),
                "dd" => new DateToken("dd"),
                "index" => new IndexToken(0),
                _ => ParsePaddedIndex(token)
            };
            if (part == null)
            {
                return false;
            }

            if (literal.Length > 0)
            {
                parts.Add(new Literal(literal.ToString()));
                literal.Clear();
            }
            parts.Add(part);
            i = close + 1;
        }
        if (literal.Length > 0)
        {
            parts.Add(new Literal(literal.ToString()));
        }

        result = new MilestoneCodeTemplate(template, parts);
        return true;
    }

    /// <summary>
    /// Whether the template contains an index token (needed for unique codes in a list).
    /// </summary>
    public bool HasIndex => _parts.Any(p => p is IndexToken);

    /// <summary>
    /// Generate the code for a participant.
    /// </summary>
    public string Generate(int index, DateOnly date)
    {
        StringBuilder sb = new();
        foreach (Part part in _parts)
        {
            switch (part)
            {
                case Literal l:
                    sb.Append(l.Text);
                    break;
                case DateToken d:
                    sb.Append(date.ToString(d.Format, CultureInfo.InvariantCulture));
                    break;
                case IndexToken t:
                    sb.Append(index.ToString(t.Padding > 0 ? new string('0', t.Padding) : "0", CultureInfo.InvariantCulture));
                    break;
            }
        }
        string code = sb.ToString().Trim();
        if (code.Length == 0 || code.Length > MaxCodeLength)
        {
            throw new DomainException(DomainErrors.MilestoneEventInvalidTemplate);
        }
        return code;
    }

    private static Part? ParsePaddedIndex(string token)
    {
        const string prefix = "index:";
        if (!token.StartsWith(prefix, StringComparison.Ordinal))
        {
            return null;
        }
        string pad = token[prefix.Length..];
        if (pad.Length is < 1 or > 9 || pad.Any(ch => ch != '0'))
        {
            return null;
        }
        return new IndexToken(pad.Length);
    }
}
