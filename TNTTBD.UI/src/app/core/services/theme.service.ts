import { Injectable, signal, effect } from '@angular/core';

const STORAGE_KEY = 'tnttbd.theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  readonly darkMode = signal<boolean>(this.loadInitial());

  constructor() {
    effect(() => {
      const isDark = this.darkMode();
      document.documentElement.classList.toggle('dark-theme', isDark);
      localStorage.setItem(STORAGE_KEY, isDark ? 'dark' : 'light');
    });
  }

  toggle(): void {
    this.darkMode.update((v) => !v);
  }

  private loadInitial(): boolean {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored === 'dark') return true;
    if (stored === 'light') return false;
    return window.matchMedia?.('(prefers-color-scheme: dark)').matches ?? false;
  }
}
