import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PhotoService {
  private http = inject(HttpClient);

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
}
