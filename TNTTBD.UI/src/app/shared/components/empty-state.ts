import { Component, input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [CommonModule, MatIconModule, TranslatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="empty-state-container flex-col">
      <div class="icon-circle">
        <mat-icon>{{ icon() }}</mat-icon>
      </div>
      <h3 class="title">{{ titleKey() | t }}</h3>
      @if (descKey()) {
        <p class="description">{{ descKey()! | t }}</p>
      }
      <div class="actions">
        <ng-content></ng-content>
      </div>
    </div>
  `,
  styles: [
    `
      .empty-state-container {
        align-items: center;
        justify-content: center;
        padding: 3rem 1.5rem;
        text-align: center;
        background: var(--mat-sys-surface-container-low);
        border-radius: 8px;
        margin: 1rem 0;
      }
      .icon-circle {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 64px;
        height: 64px;
        border-radius: 50%;
        background: var(--mat-sys-surface-variant);
        color: var(--mat-sys-on-surface-variant);
        margin-bottom: 1rem;
        
        mat-icon {
          font-size: 32px;
          height: 32px;
          width: 32px;
        }
      }
      .title {
        margin: 0 0 0.5rem;
        font: var(--mat-sys-title-medium);
        color: var(--mat-sys-on-surface);
      }
      .description {
        margin: 0 0 1.5rem;
        font: var(--mat-sys-body-medium);
        color: var(--mat-sys-on-surface-variant);
        max-width: 400px;
      }
      .actions {
        display: flex;
        gap: 0.5rem;
        justify-content: center;
        flex-wrap: wrap;
      }
    `
  ]
})
export class EmptyState {
  icon = input<string>('folder_open');
  titleKey = input<string>('common.noData');
  descKey = input<string | undefined>();
}
