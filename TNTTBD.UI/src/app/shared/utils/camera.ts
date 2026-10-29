import { DestroyRef, inject, signal, Signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { BreakpointObserver } from '@angular/cdk/layout';
import { map } from 'rxjs';

export function injectIsTouch(): Signal<boolean> {
  return toSignal(
    inject(BreakpointObserver)
      .observe('(pointer: coarse)')
      .pipe(map(result => result.matches)),
    { initialValue: false },
  );
}

export function injectHasCamera(): Signal<boolean> {
  const hasCamera = signal(false);
  const media = navigator.mediaDevices;

  if (!media?.enumerateDevices) {
    return hasCamera.asReadonly();
  }

  let alive = true;
  const refresh = (): void => {
    media.enumerateDevices()
      .then(devices => {
        if (alive) {
          hasCamera.set(devices.some(device => device.kind === 'videoinput'));
        }
      })
      .catch(() => undefined);
  };

  refresh();
  media.addEventListener('devicechange', refresh);
  inject(DestroyRef).onDestroy(() => {
    alive = false;
    media.removeEventListener('devicechange', refresh);
  });

  return hasCamera.asReadonly();
}
