import { Pipe, PipeTransform, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TranslationService } from '../../core/i18n/translation.service';

/**
 * Formats IsoDate ('YYYY-MM-DD') or IsoDateTime (ISO 8601) according to active locale.
 * Type 'date' = calendar only, 'datetime' = date + time.
 */
@Pipe({
  name: 'dateLocale',
  standalone: true,
  pure: false,
})
export class DateLocalePipe implements PipeTransform {
  private t = inject(TranslationService);
  private datePipe = new DatePipe('en-US');

  transform(value: string | null | undefined, type: 'date' | 'datetime' = 'date'): string {
    if (!value) return '';
    const lang = this.t.currentLang();
    const locale = lang === 'vi' ? 'vi-VN' : 'en-US';
    const fmt = type === 'date' ? 'shortDate' : 'short';
    // For IsoDate (YYYY-MM-DD), append T00:00 to avoid TZ shift in some browsers
    const input = type === 'date' && /^\d{4}-\d{2}-\d{2}$/.test(value) ? value + 'T00:00:00' : value;
    const datePipe = new DatePipe(locale);
    return datePipe.transform(input, fmt, undefined, locale) ?? '';
  }
}
