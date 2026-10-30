import { ChangeDetectionStrategy, Component, effect, inject, input, signal } from '@angular/core';
import { PhotoService } from '../../../core/services/photo.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';

@Component({
  selector: 'app-photo-image',
  standalone: true,
  imports: [TranslatePipe],
  templateUrl: './photo-image.html',
  styleUrl: './photo-image.scss',
  host: { '[class.full-size]': 'fullSize()' },
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotoImage {
  private photos = inject(PhotoService);
  readonly url = input.required<string>();
  readonly alt = input.required<string>();
  readonly fullSize = input(false);
  protected src = signal('');
  protected failed = signal(false);

  constructor() {
    effect((onCleanup) => {
      const url = this.photos.url(this.url());
      this.src.set('');
      this.failed.set(false);
      // Public external images do not need our API's bearer token.
      if (!this.photos.isApiPhoto(url)) {
        this.src.set(url);
        return;
      }
      let objectUrl: string | undefined;
      const subscription = this.photos.load(url).subscribe({
        next: (blob) => {
          objectUrl = URL.createObjectURL(blob);
          this.src.set(objectUrl);
        },
        error: () => this.failed.set(true),
      });
      onCleanup(() => {
        subscription.unsubscribe();
        if (objectUrl) URL.revokeObjectURL(objectUrl);
      });
    });
  }
}
