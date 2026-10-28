import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { PhotoPurpose, PhotoVisibility } from '../../shared/enums';
import { ApiService } from './api.service';

export type PhotoEntityType = 'catechists' | 'youths';

export interface PhotoPermissions {
  canManage: boolean;
}

@Injectable({ providedIn: 'root' })
export class PhotoService {
  private http = inject(HttpClient);
  private api = inject(ApiService);

  url(value: string): string {
    const api = new URL(environment.apiBaseUrl, location.origin);
    return new URL(value, api.origin).href;
  }

  isApiPhoto(url: string): boolean {
    const api = new URL(environment.apiBaseUrl, location.origin);
    const photo = new URL(url);
    return photo.origin === api.origin && photo.pathname.startsWith(`${api.pathname}/files/`);
  }

  load(url: string) {
    return this.http.get(url, { responseType: 'blob' });
  }

  permissions(entityType: PhotoEntityType, entityId: string) {
    return this.api.get<PhotoPermissions>(`/${entityType}/photos/permissions`, {
      [entityType === 'catechists' ? 'catechistId' : 'youthId']: entityId,
    });
  }

  updateSettings(
    entityType: PhotoEntityType,
    entityId: string,
    photoId: string,
    purpose: PhotoPurpose,
    visibility: PhotoVisibility,
  ): Promise<void> {
    return this.api.patchAsync(`/${entityType}/photos`, { ...this.target(entityType, entityId, photoId), purpose, visibility });
  }

  remove(entityType: PhotoEntityType, entityId: string, photoId: string): Promise<void> {
    return this.api.deleteAsync(`/${entityType}/photos`, this.target(entityType, entityId, photoId));
  }

  private target(entityType: PhotoEntityType, entityId: string, photoId: string) {
    return { [entityType === 'catechists' ? 'catechistId' : 'eucharisticYouthId']: entityId, photoId };
  }
}
