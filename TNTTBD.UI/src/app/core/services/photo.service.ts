import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PhotoService {
  url(storageKey: string | null | undefined): string {
    if (!storageKey) return '';
    if (storageKey.startsWith('http://') || storageKey.startsWith('https://')) return storageKey;
    const prefix = environment.photoBaseUrl.endsWith('/')
      ? environment.photoBaseUrl
      : environment.photoBaseUrl + '/';
    return prefix + storageKey.replace(/^\/+/, '');
  }
}
