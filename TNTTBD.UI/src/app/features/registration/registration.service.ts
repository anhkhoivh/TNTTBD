import { Injectable, inject } from '@angular/core';
import { ApiService } from '../../core/services/api.service';

export interface RegistrationPayload {
  youth: unknown;
  contacts: unknown[];
}

@Injectable()
export class RegistrationService {
  private api = inject(ApiService);

  async submit(payload: RegistrationPayload): Promise<void> {
    await this.api.postAsync('/registration/youth', payload);
  }
}
