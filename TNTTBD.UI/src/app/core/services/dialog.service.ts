import { Injectable, inject } from '@angular/core';
import { ComponentType } from '@angular/cdk/overlay';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { ConfirmDialog, ConfirmData } from '../../shared/components/confirm-dialog';

/** Promise-based wrapper over MatDialog, whose only result API is afterClosed(). */
@Injectable({ providedIn: 'root' })
export class DialogService {
  private dialog = inject(MatDialog);

  open<C, D = unknown, R = unknown>(
    component: ComponentType<C>,
    config?: MatDialogConfig<D>,
  ): Promise<R | undefined> {
    return firstValueFrom(this.dialog.open<C, D, R>(component, config).afterClosed());
  }

  async confirm(data: ConfirmData): Promise<boolean> {
    const ok = await this.open<ConfirmDialog, ConfirmData, boolean>(ConfirmDialog, { data });
    return ok === true;
  }

  confirmDelete(message: string): Promise<boolean> {
    return this.confirm({ message, confirmKey: 'common.delete', destructive: true });
  }
}
