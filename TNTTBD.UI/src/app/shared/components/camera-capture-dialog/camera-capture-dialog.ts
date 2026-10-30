import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  ElementRef,
  inject,
  signal,
  viewChild,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogActions,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';

@Component({
  selector: 'app-camera-capture-dialog',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    TranslatePipe,
  ],
  templateUrl: './camera-capture-dialog.html',
  styleUrl: './camera-capture-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CameraCaptureDialog {
  private readonly ref = inject<MatDialogRef<CameraCaptureDialog, File | null>>(MatDialogRef);
  private readonly destroyRef = inject(DestroyRef);
  readonly video = viewChild.required<ElementRef<HTMLVideoElement>>('video');
  protected readonly ready = signal(false);
  protected readonly error = signal<string | null>(null);
  private stream: MediaStream | null = null;

  constructor() {
    void this.start();
    this.destroyRef.onDestroy(() => this.stop());
  }

  protected capture(): void {
    if (!this.ready()) return;

    const video = this.video().nativeElement;
    if (!video.videoWidth) return;

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const context = canvas.getContext('2d');
    if (!context) {
      this.error.set('photo.cameraFailed');
      return;
    }

    context.drawImage(video, 0, 0);
    canvas.toBlob(blob => {
      if (!blob) {
        this.error.set('photo.cameraFailed');
        return;
      }

      this.ref.close(new File([blob], `photo-${Date.now()}.jpg`, { type: 'image/jpeg' }));
    }, 'image/jpeg');
  }

  protected cancel(): void {
    this.ref.close(null);
  }

  private async start(): Promise<void> {
    try {
      this.stream = await navigator.mediaDevices.getUserMedia({
        video: { facingMode: 'environment', width: { ideal: 1920 }, height: { ideal: 1080 } },
        audio: false,
      });

      if (this.destroyRef.destroyed) {
        this.stop();
        return;
      }

      const video = this.video().nativeElement;
      video.srcObject = this.stream;
      await video.play();
      this.ready.set(true);
    } catch (error: unknown) {
      this.stop();
      this.error.set(this.cameraError(error));
    }
  }

  private stop(): void {
    this.stream?.getTracks().forEach(track => track.stop());
    this.stream = null;
  }

  private cameraError(error: unknown): string {
    const name = typeof error === 'object' && error !== null && 'name' in error
      && typeof error.name === 'string'
      ? error.name
      : '';
    if (name === 'NotAllowedError' || name === 'PermissionDeniedError') {
      return 'photo.cameraDenied';
    }
    if (name === 'NotFoundError' || name === 'DevicesNotFoundError') {
      return 'photo.cameraNotFound';
    }
    return 'photo.cameraFailed';
  }
}
