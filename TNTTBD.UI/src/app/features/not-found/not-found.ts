import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink, MatButtonModule, MatIconModule, TranslatePipe],
  template: `
    <div class="not-found">
      <mat-icon class="icon">sentiment_dissatisfied</mat-icon>
      <h1>404</h1>
      <p>{{ 'notFound.message' | t }}</p>
      <a mat-flat-button color="primary" routerLink="/">
        <mat-icon>home</mat-icon>
        {{ 'notFound.backHome' | t }}
      </a>
    </div>
  `,
  styles: [
    `
      .not-found {
        min-height: 100vh;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        gap: 1rem;
        padding: 2rem;
        text-align: center;
      }
      .icon {
        font-size: 96px;
        width: 96px;
        height: 96px;
        color: var(--mat-sys-on-surface-variant);
      }
      h1 {
        font-size: 4rem;
        margin: 0;
      }
    `,
  ],
})
export class NotFound {}
