import { Injectable, computed, inject } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { ApiService } from '../../core/services/api.service';
import { PaginatedResult, User } from '../../shared/models';

function emptyPage(): PaginatedResult<User> {
  return { items: [], total: 0, page: 1, pageSize: 100 };
}

@Injectable()
export class AdminService {
  private api = inject(ApiService);

  readonly usersRes = rxResource<PaginatedResult<User>, void>({
    stream: () => this.api.list<User>('/users', { page: 1, pageSize: 100 }),
  });
  readonly users = computed(() => this.usersRes.value() ?? emptyPage());

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
