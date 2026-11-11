import { IsoDate, MilestonePrerequisiteWarning } from '../../shared/models';
import { MILESTONE_TYPE_KEYS } from '../../shared/enums';

/** One line per warning, e.g. "No First Penance record". */
export function formatPrerequisiteWarnings(
  warnings: readonly MilestonePrerequisiteWarning[] | null | undefined,
  t: (key: string) => string,
): string {
  return (warnings ?? [])
    .map((w) => t(`milestone.warning.${w.issue}`).replace('{type}', t(MILESTONE_TYPE_KEYS[w.type])))
    .join('\n');
}

export function toIsoDate(d: Date | null | undefined): IsoDate | null {
  if (!d) return null;
  const yyyy = d.getFullYear();
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const dd = String(d.getDate()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}`;
}

export function fromIsoDate(value: IsoDate | null | undefined): Date | null {
  return value ? new Date(value + 'T00:00:00') : null;
}

export function downloadBlob(blob: Blob, fileName: string): void {
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = fileName.replace(/[<>:"/\\|?*]/g, '-');
  link.click();
  setTimeout(() => URL.revokeObjectURL(url), 1000);
}

type TemplatePart =
  | { kind: 'literal'; text: string }
  | { kind: 'date'; format: 'yyyy' | 'yy' | 'MM' | 'dd' }
  | { kind: 'index'; padding: number };

export interface ParsedCodeTemplate {
  hasIndex: boolean;
  generate(index: number, date: IsoDate): string;
}

export const MAX_CODE_TEMPLATE_LENGTH = 100;

/**
 * Client-side mirror of the server's MilestoneCodeTemplate, used for instant preview.
 * Tokens: {yyyy} {yy} {MM} {dd} {index} {index:000}. Returns null when invalid.
 */
export function parseCodeTemplate(template: string | null | undefined): ParsedCodeTemplate | null {
  const t = (template ?? '').trim();
  if (!t || t.length > MAX_CODE_TEMPLATE_LENGTH) return null;

  const parts: TemplatePart[] = [];
  let literal = '';
  let i = 0;
  while (i < t.length) {
    const c = t[i];
    if (c === '}') return null;
    if (c !== '{') {
      literal += c;
      i++;
      continue;
    }
    const close = t.indexOf('}', i + 1);
    if (close < 0) return null;
    const token = t.substring(i + 1, close);
    if (token.includes('{')) return null;

    let part: TemplatePart | null = null;
    if (token === 'yyyy' || token === 'yy' || token === 'MM' || token === 'dd') {
      part = { kind: 'date', format: token };
    } else if (token === 'index') {
      part = { kind: 'index', padding: 0 };
    } else if (/^index:0{1,9}$/.test(token)) {
      part = { kind: 'index', padding: token.length - 'index:'.length };
    }
    if (!part) return null;

    if (literal) {
      parts.push({ kind: 'literal', text: literal });
      literal = '';
    }
    parts.push(part);
    i = close + 1;
  }
  if (literal) parts.push({ kind: 'literal', text: literal });

  return {
    hasIndex: parts.some((p) => p.kind === 'index'),
    generate(index: number, date: IsoDate): string {
      const [yyyy, mm, dd] = date.split('-');
      return parts
        .map((p) => {
          if (p.kind === 'literal') return p.text;
          if (p.kind === 'index') return String(index).padStart(p.padding, '0');
          switch (p.format) {
            case 'yyyy': return yyyy;
            case 'yy': return yyyy.slice(-2);
            case 'MM': return mm;
            case 'dd': return dd;
          }
        })
        .join('')
        .trim();
    },
  };
}
