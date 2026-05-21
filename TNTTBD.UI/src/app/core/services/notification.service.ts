import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private snack = inject(MatSnackBar);

  success(message: string): void {
    this.snack.open(message, 'OK', { duration: 3000, panelClass: 'snack-success' });
  }

  error(message: string): void {
    this.snack.open(message, 'OK', { duration: 5000, panelClass: 'snack-error' });
  }

  info(message: string): void {
    this.snack.open(message, 'OK', { duration: 3000 });
  }
}
