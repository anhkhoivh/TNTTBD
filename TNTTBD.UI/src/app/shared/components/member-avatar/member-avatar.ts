import { ChangeDetectionStrategy, Component, computed, effect, inject, input, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { PhotoEntityType, PhotoService } from '../../../core/services/photo.service';

export type MemberAvatarSize = 'sm' | 'lg';

@Component({
  selector: 'app-member-avatar',
  standalone: true,
  imports: [MatIconModule],
  templateUrl: './member-avatar.html',
  styleUrl: './member-avatar.scss',
  host: {
    class: 'member-avatar',
    '[class.lg]': "size() === 'lg'",
    role: 'img',
    '[attr.aria-label]': 'alt()',
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MemberAvatar {
  private photos = inject(PhotoService);
  readonly entityType = input.required<PhotoEntityType>();
  readonly entityId = input.required<string>();
  readonly alt = input('');
  readonly size = input<MemberAvatarSize>('sm');
  protected src = signal<string | null>(null);
  protected icon = computed(() => (this.entityType() === 'youths' ? 'face' : 'person'));

  constructor() {
    effect((onCleanup) => {
      const entityType = this.entityType();
      const entityId = this.entityId();
      this.photos.profileChanges();
      this.src.set(null);
      if (!entityId) return;
      const subscription = this.photos.profilePhoto(entityType, entityId).subscribe((src) => this.src.set(src));
      onCleanup(() => subscription.unsubscribe());
    });
  }
}
