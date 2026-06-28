import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

export type Lang = 'vi' | 'en';
const STORAGE_KEY = 'tnttbd.lang';

@Injectable({ providedIn: 'root' })
export class TranslationService {
  private http = inject(HttpClient);
  private dict = signal<Record<string, string>>({});
  readonly lang = signal<Lang>(this.loadInitial());

  readonly currentLang = computed(() => this.lang());

  async init(): Promise<void> {
    await this.load(this.lang());
  }

  async setLang(lang: Lang): Promise<void> {
    this.lang.set(lang);
    localStorage.setItem(STORAGE_KEY, lang);
    await this.load(lang);
  }

  t(key: string): string {
    return this.dict()[key] ?? key;
  }

  private async load(lang: Lang): Promise<void> {
    const data = await firstValueFrom(
      this.http.get<Record<string, string>>(`/assets/i18n/${lang}.json`),
    );
    this.dict.set(data);
  }

  private loadInitial(): Lang {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored === 'vi' || stored === 'en') return stored;
    return 'vi';
  }
}
