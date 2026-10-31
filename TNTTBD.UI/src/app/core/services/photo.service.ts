import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PhotoPurpose } from '../../shared/enums';
import { AuthService } from '../auth/auth.service';
import { ApiService } from './api.service';

export type PhotoEntityType = 'catechists' | 'youths';

export interface PhotoPermissions {
  canManage: boolean;
}

interface ProfileEntry {
  src$: Observable<string | null>;
  objectUrl?: string;
}

@Injectable({ providedIn: 'root' })
export class PhotoService {
  private http = inject(HttpClient);
  private api = inject(ApiService);
  private auth = inject(AuthService);
  private profiles = new Map<string, ProfileEntry>();
  private profilesOwner: string | null = null;
  private _profileChanges = signal(0);
  /** Bumped whenever a cached profile photo is invalidated, so avatars reload. */
  readonly profileChanges = this._profileChanges.asReadonly();

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

  /**
   * Object URL of the member's profile photo, or null when there is none or the
   * current user may not view it. Downloaded once per member and signed-in user.
   */
  profilePhoto(entityType: PhotoEntityType, entityId: string): Observable<string | null> {
    const owner = this.auth.getStoredUserId();
    if (owner !== this.profilesOwner) {
      this.clearProfiles();
      this.profilesOwner = owner;
    }
    const key = `${entityType}:${entityId}`;
    const cached = this.profiles.get(key);
    if (cached) return cached.src$;

    const entry: ProfileEntry = { src$: of(null) };
    entry.src$ = this.http
      .get(`${environment.apiBaseUrl}/${entityType}/profile-photo`, {
        params: { [entityType === 'catechists' ? 'catechistId' : 'youthId']: entityId },
        responseType: 'blob',
        observe: 'response',
      })
      .pipe(
        map((response) => (response.status === 204 || !response.body?.size ? null : URL.createObjectURL(response.body))),
        tap((objectUrl) => (entry.objectUrl = objectUrl ?? undefined)),
        catchError(() => of(null)),
        shareReplay(1),
      );
    this.profiles.set(key, entry);
    return entry.src$;
  }

  /** Forget a cached profile photo after it was changed, uploaded or removed. */
  invalidateProfile(entityType: PhotoEntityType, entityId: string): void {
    const key = `${entityType}:${entityId}`;
    const entry = this.profiles.get(key);
    if (entry?.objectUrl) URL.revokeObjectURL(entry.objectUrl);
    this.profiles.delete(key);
    this._profileChanges.update((n) => n + 1);
  }

  private clearProfiles(): void {
    for (const entry of this.profiles.values()) {
      if (entry.objectUrl) URL.revokeObjectURL(entry.objectUrl);
    }
    this.profiles.clear();
  }

  permissions(entityType: PhotoEntityType, entityId: string) {
    return this.api.get<PhotoPermissions>(`/${entityType}/photos/permissions`, {
      [entityType === 'catechists' ? 'catechistId' : 'youthId']: entityId,
    });
  }

  async updateSettings(
    entityType: PhotoEntityType,
    entityId: string,
    photoId: string,
    purpose: PhotoPurpose,
  ): Promise<void> {
    await this.api.patchAsync(`/${entityType}/photos`, { ...this.target(entityType, entityId, photoId), purpose });
    this.invalidateProfile(entityType, entityId);
  }

  async remove(entityType: PhotoEntityType, entityId: string, photoId: string): Promise<void> {
    await this.api.deleteAsync(`/${entityType}/photos`, this.target(entityType, entityId, photoId));
    this.invalidateProfile(entityType, entityId);
  }

  private target(entityType: PhotoEntityType, entityId: string, photoId: string) {
    return { [entityType === 'catechists' ? 'catechistId' : 'eucharisticYouthId']: entityId, photoId };
  }
}
