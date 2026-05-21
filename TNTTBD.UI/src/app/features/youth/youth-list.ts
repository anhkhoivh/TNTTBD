import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ApiService } from '../../core/services/api.service';
import { EucharisticYouth, PaginatedResult } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';

@Component({
  selector: 'app-youth-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './youth-list.html',
  styleUrl: './youth-list.scss',
})
export class YouthList {
  private api = inject(ApiService);

  protected data = toSignal(this.api.list<EucharisticYouth>('/youths'), {
    initialValue: { items: [], total: 0, page: 1, pageSize: 50 } as PaginatedResult<EucharisticYouth>,
  });

  protected displayedColumns = ['code', 'fullName', 'dateOfBirth', 'gender', 'actions'];
}
