import { Component, inject, signal, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/auth/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { CatechismSessionAttendance } from '../models';
import { AttendanceStatus } from '../enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../pipes/full-name.pipe';

export interface AttendanceDialogData {
  classId: string;
  sessionId: string;
}

interface Row {
  enrollmentId: string;
  youthFullName: any;
  status: AttendanceStatus;
}

@Component({
  selector: 'app-attendance-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatTableModule,
    MatButtonToggleModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ 'attendance.title' | t }}</h2>
    <mat-dialog-content>
      @if (loading()) {
        <div class="center"><mat-spinner diameter="40"></mat-spinner></div>
      } @else {
        <table mat-table [dataSource]="rows()" class="full-width">
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>{{ 'youth.fullName' | t }}</th>
            <td mat-cell *matCellDef="let row">
              {{ row.youthFullName | fullName }}
            </td>
          </ng-container>
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let row; let i = index">
              <mat-button-toggle-group
                [value]="row.status"
                (change)="setStatus(i, $event.value)"
              >
                @for (s of statuses; track s.v) {
                  <mat-button-toggle [value]="s.v">
                    <mat-icon>{{ s.icon }}</mat-icon>
                  </mat-button-toggle>
                }
              </mat-button-toggle-group>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="['name', 'status']"></tr>
          <tr mat-row *matRowDef="let row; columns: ['name', 'status']"></tr>
        </table>
      }
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="ref.close()">{{ 'common.cancel' | t }}</button>
      <button mat-flat-button color="primary" (click)="save()" [disabled]="saving()">
        {{ 'common.save' | t }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [`.center { display: flex; justify-content: center; padding: 2rem; }`],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AttendanceDialog implements OnInit {
  private api = inject(ApiService);
  private auth = inject(AuthService);
  private notify = inject(NotificationService);
  private cdr = inject(ChangeDetectorRef);
  protected ref = inject(MatDialogRef<AttendanceDialog>);
  private data: AttendanceDialogData = inject(MAT_DIALOG_DATA);

  protected readonly statuses = [
    { v: AttendanceStatus.Present, icon: 'check_circle' },
    { v: AttendanceStatus.Absent, icon: 'cancel' },
    { v: AttendanceStatus.Excused, icon: 'event_busy' },
  ];

  protected loading = signal(true);
  protected saving = signal(false);
  protected rows = signal<Row[]>([]);

  ngOnInit(): void {
    this.api
      .get<CatechismSessionAttendance[]>('/sessions/attendance-sheet', { sessionId: this.data.sessionId })
      .subscribe((items) => {
        this.rows.set(
          (items ?? []).map((a) => ({
            enrollmentId: a.catechismClassEucharisticYouthId,
            youthFullName: a.youthFullName,
            status: a.attendanceStatus ?? AttendanceStatus.None,
          })),
        );
        this.loading.set(false);
        this.cdr.markForCheck();
      });
  }

  protected setStatus(index: number, status: AttendanceStatus): void {
    const current = this.rows();
    const updated = [...current];
    updated[index] = { ...updated[index], status };
    this.rows.set(updated);
  }

  protected save(): void {
    this.saving.set(true);
    const payload = {
      sessionId: this.data.sessionId,
      markedByCatechistId: this.auth.currentUser()?.catechistId,
      attendanceItems: this.rows()
        .filter((r) => r.status !== AttendanceStatus.None)
        .map((r) => ({
          catechismClassEucharisticYouthId: r.enrollmentId,
          attendanceStatus: r.status,
        })),
    };
    this.api.post('/sessions/attendances', payload).subscribe({
      next: () => {
        this.saving.set(false);
        this.notify.success('Saved');
        this.ref.close(true);
      },
      error: () => {
        this.saving.set(false);
        this.notify.error('Save failed');
      },
    });
  }
}
