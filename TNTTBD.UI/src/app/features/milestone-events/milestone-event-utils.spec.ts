import { MilestoneType } from '../../shared/enums';
import { formatPrerequisiteWarnings, fromIsoDate, parseCodeTemplate, toIsoDate } from './milestone-event-utils';

describe('parseCodeTemplate', () => {
  it('generates codes with date tokens and padded index', () => {
    const t = parseCodeTemplate('RT-{yyyy}{MM}{dd}-{index:000}');
    expect(t).not.toBeNull();
    expect(t!.hasIndex).toBe(true);
    expect(t!.generate(7, '2026-05-03')).toBe('RT-20260503-007');
    expect(t!.generate(1234, '2026-05-03')).toBe('RT-20260503-1234');
  });

  it('supports plain index and two-digit year', () => {
    const t = parseCodeTemplate('{yy}/{index}');
    expect(t!.generate(12, '2026-01-09')).toBe('26/12');
  });

  it('reports templates without index', () => {
    expect(parseCodeTemplate('TS-{yyyy}')!.hasIndex).toBe(false);
  });

  it.each(['', '   ', '{foo}', '{index', 'a}b', '{{index}}', '{index:0000000000}', 'x'.repeat(101)])(
    'rejects invalid template %j',
    (tpl) => expect(parseCodeTemplate(tpl)).toBeNull(),
  );
});

describe('iso date helpers', () => {
  it('round-trips local dates', () => {
    expect(toIsoDate(fromIsoDate('2026-12-31'))).toBe('2026-12-31');
    expect(toIsoDate(null)).toBeNull();
    expect(fromIsoDate(null)).toBeNull();
  });
});

describe('formatPrerequisiteWarnings', () => {
  const t = (k: string) => (k.startsWith('milestone.warning.') ? `${k.slice(18)}:{type}` : `[${k}]`);

  it('formats one line per warning with the translated type', () => {
    const text = formatPrerequisiteWarnings(
      [
        { type: MilestoneType.FirstPenance, issue: 'Missing' },
        { type: MilestoneType.Baptism, issue: 'DatedAfter' },
      ],
      t,
    );
    expect(text).toBe('Missing:[milestone.firstPenance]\nDatedAfter:[milestone.baptism]');
  });

  it('returns empty text for no warnings', () => {
    expect(formatPrerequisiteWarnings([], t)).toBe('');
    expect(formatPrerequisiteWarnings(null, t)).toBe('');
  });
});
