import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { PaginatedResult, User } from '../../shared/models';
import { UserRole } from '../../shared/enums';

function emptyPage(): PaginatedResult<User> {
  return { items: [], total: 0, page: 1, pageSize: 25 };
}

export interface UserListParams {
  search: string;
  role: UserRole;
  page: number;
  pageSize: number;
}

@Injectable()
export class AdminService {
  private api = inject(ApiService);

  readonly listParams = signal<UserListParams>({
    search: '',
    role: UserRole.Admin,
    page: 1,
    pageSize: 25,
  });
  readonly usersRes = rxResource<PaginatedResult<User>, UserListParams>({
    params: () => this.listParams(),
    stream: ({ params }) =>
      this.api
        .list<User>('/users', {
          page: params.page,
          pageSize: params.pageSize,
          search: params.search.trim() || undefined,
          role: params.role,
        })
        .pipe(catchError(() => of(emptyPage()))),
  });
  readonly users = computed(() => this.usersRes.value() ?? emptyPage());
  readonly isLoading = computed(() => this.usersRes.isLoading());

  async createUser(payload: unknown): Promise<void> {
    await this.api.postAsync('/users', payload);
    this.usersRes.reload();
  }

  async updateUser(payload: unknown): Promise<void> {
    await this.api.patchAsync('/users', payload);
    this.usersRes.reload();
  }

  async toggleActive(userId: string, isActive: boolean): Promise<void> {
    await this.api.patchAsync('/users', { id: userId, isActive });
    this.usersRes.reload();
  }

  async deleteUser(userId: string): Promise<void> {
    await this.api.deleteAsync('/users', { id: userId });
    this.usersRes.reload();
  }

  async resetPassword(payload: unknown): Promise<void> {
    await this.api.postAsync('/users/reset-password', payload);
  }
}
