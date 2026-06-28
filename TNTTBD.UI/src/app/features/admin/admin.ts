import { Component, inject , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../core/services/api.service';
import { User } from '../../shared/models';
import { UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatChipsModule,
    MatIconModule,
    TranslatePipe,
  ],
  templateUrl: './admin.html',
  styleUrl: './admin.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminPage {
  private api = inject(ApiService);

  protected readonly UserRole = UserRole;

  protected data = toSignal(this.api.list<User>('/users'), {
    initialValue: { items: [], total: 0, page: 1, pageSize: 50 },
  });

  protected cols = ['userName', 'email', 'role', 'active'];

  protected roleName(role: UserRole): string {
    return UserRole[role] ?? '';
  }
}

