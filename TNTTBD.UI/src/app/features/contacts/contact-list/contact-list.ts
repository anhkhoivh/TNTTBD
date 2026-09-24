import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { EmptyState } from '../../../shared/components/empty-state';
import { ContactService } from '../contact.service';

@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    TranslatePipe,
    FullNamePipe,
    EmptyState,
  ],
  templateUrl: './contact-list.html',
  styleUrl: './contact-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ContactService],
})
export class ContactList {
  private contactService = inject(ContactService);

  protected data = this.contactService.list;

  protected displayedColumns = ['fullName', 'phoneNumber', 'email'];
  protected searchQuery = signal('');

  onPageChange(event: PageEvent) {
    this.contactService.listParams.update((p) => ({
      ...p,
      page: event.pageIndex + 1,
      pageSize: event.pageSize,
    }));
  }

  onSearch(query: string) {
    this.searchQuery.set(query);
    this.contactService.listParams.update((p) => ({ ...p, search: query, page: 1 }));
  }
}
