import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';

import { EmptyState } from '../../shared/components/empty-state';
import { FormsModule } from '@angular/forms';
import { YouthService } from './youth.service';

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
    MatPaginatorModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
    EmptyState,
    FormsModule
  ],
  templateUrl: './youth-list.html',
  styleUrl: './youth-list.scss',

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [YouthService],
})
export class YouthList {
  private youthService = inject(YouthService);

  protected data = this.youthService.list;

  protected displayedColumns = ['code', 'fullName', 'dateOfBirth', 'gender'];
  protected searchQuery = signal('');

  onPageChange(event: PageEvent) {
    this.youthService.listParams.update((p) => ({
      ...p,
      page: event.pageIndex + 1,
      pageSize: event.pageSize,
    }));
  }

  onSearch(query: string) {
    this.searchQuery.set(query);
    this.youthService.listParams.update((p) => ({ ...p, search: query, page: 1 }));
  }
}
