import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedResult } from '../../shared/models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private base = environment.apiBaseUrl;

  list<T>(path: string, params?: Record<string, string | number | undefined>): Observable<PaginatedResult<T>> {
    return this.http.get<PaginatedResult<T>>(`${this.base}${path}`, { params: this.toParams(params) });
  }

  get<T>(path: string, params?: Record<string, string | number | undefined>): Observable<T> {
    return this.http.get<T>(`${this.base}${path}`, { params: this.toParams(params) });
  }

  post<T>(path: string, body: unknown): Observable<T> {
    return this.http.post<T>(`${this.base}${path}`, body);
  }

  postBlob(path: string, body: unknown): Observable<Blob> {
    return this.http.post(`${this.base}${path}`, body, { responseType: 'blob' });
  }

  patch<T>(path: string, body: unknown): Observable<T> {
    return this.http.patch<T>(`${this.base}${path}`, body);
  }

  delete<T>(path: string, body: unknown): Observable<T> {
    return this.http.delete<T>(`${this.base}${path}`, { body });
  }

  getAsync<T>(path: string, params?: Record<string, string | number | undefined>): Promise<T> {
    return firstValueFrom(this.get<T>(path, params));
  }

  postAsync<T>(path: string, body: unknown): Promise<T> {
    return firstValueFrom(this.post<T>(path, body));
  }

  postBlobAsync(path: string, body: unknown): Promise<Blob> {
    return firstValueFrom(this.postBlob(path, body));
  }

  getBlobAsync(path: string, params?: Record<string, string | number | undefined>): Promise<Blob> {
    return firstValueFrom(
      this.http.get(`${this.base}${path}`, { params: this.toParams(params), responseType: 'blob' }),
    );
  }

  patchAsync<T>(path: string, body: unknown): Promise<T> {
    return firstValueFrom(this.patch<T>(path, body));
  }

  deleteAsync<T>(path: string, body: unknown): Promise<T> {
    return firstValueFrom(this.delete<T>(path, body));
  }

  private toParams(params?: Record<string, string | number | undefined>): HttpParams {
    let p = new HttpParams();
    if (!params) return p;
    for (const key of Object.keys(params)) {
      const v = params[key];
      if (v !== undefined && v !== null) p = p.set(key, String(v));
    }
    return p;
  }
}
