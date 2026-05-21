import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export type BadgeVariant = 'success' | 'warn' | 'error' | 'info' | 'neutral';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  template: `
    <span class="badge" [class]="variant()">{{ labelKey() | t }}</span>
  `,
  styles: [
    `
      .badge {
        display: inline-flex;
        align-items: center;
        padding: 2px 10px;
        border-radius: 12px;
        font: var(--mat-sys-label-small);
        white-space: nowrap;
      }
      .success {
        background: var(--mat-sys-tertiary-container);
        color: var(--mat-sys-on-tertiary-container);
      }
      .warn {
        background: color-mix(in srgb, var(--mat-sys-error) 20%, transparent);
        color: var(--mat-sys-error);
      }
      .error {
        background: var(--mat-sys-error-container);
        color: var(--mat-sys-on-error-container);
      }
      .info {
        background: var(--mat-sys-primary-container);
        color: var(--mat-sys-on-primary-container);
      }
      .neutral {
        background: var(--mat-sys-surface-variant);
        color: var(--mat-sys-on-surface-variant);
      }
    `,
  ],
})
export class StatusBadge {
  /** i18n key to translate and display */
  readonly labelKey = input.required<string>();
  /** Visual variant — determines chip color */
  readonly variant = input<BadgeVariant>('neutral');
}
