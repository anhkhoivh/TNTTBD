import { Injectable, inject } from '@angular/core';
import { ComponentType } from '@angular/cdk/overlay';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatBottomSheet, MatBottomSheetConfig } from '@angular/material/bottom-sheet';
import { firstValueFrom } from 'rxjs';
import { ConfirmDialog, ConfirmData } from '../../shared/components/confirm-dialog';

/** Promise-based wrapper over MatDialog / MatBottomSheet, whose only result API is afterClosed()/afterDismissed(). */
@Injectable({ providedIn: 'root' })
export class DialogService {
  private dialog = inject(MatDialog);
  private bottomSheet = inject(MatBottomSheet);

  open<C, D = unknown, R = unknown>(
    component: ComponentType<C>,
    config?: MatDialogConfig<D>,
  ): Promise<R | undefined> {
    return firstValueFrom(this.dialog.open<C, D, R>(component, config).afterClosed());
  }

  openBottomSheet<C, D = unknown, R = unknown>(
    component: ComponentType<C>,
    config?: MatBottomSheetConfig<D>,
  ): Promise<R | undefined> {
    return firstValueFrom(this.bottomSheet.open<C, D, R>(component, config).afterDismissed());
  }

  async confirm(data: ConfirmData): Promise<boolean> {
    const ok = await this.open<ConfirmDialog, ConfirmData, boolean>(ConfirmDialog, { data });
    return ok === true;
  }

  confirmDelete(message: string): Promise<boolean> {
    return this.confirm({ message, confirmKey: 'common.delete', destructive: true });
  }
}
