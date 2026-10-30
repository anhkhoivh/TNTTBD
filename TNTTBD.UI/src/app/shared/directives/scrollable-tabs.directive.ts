import { afterNextRender, DestroyRef, Directive, ElementRef, inject, NgZone } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatTabGroup } from '@angular/material/tabs';

type ScrollDirection = -1 | 1;

@Directive({
  selector: 'mat-tab-group[appScrollableTabs]',
  standalone: true,
  host: { class: 'app-scrollable-tabs' },
})
export class ScrollableTabsDirective {
  private readonly group = inject(MatTabGroup);
  private readonly element = inject<ElementRef<HTMLElement>>(ElementRef);
  private readonly destroyRef = inject(DestroyRef);
  private readonly zone = inject(NgZone);
  private container?: HTMLElement;
  private before?: HTMLElement;
  private after?: HTMLElement;
  private frame?: number;

  constructor() {
    // Material's own arrows translate the tab list, which conflicts with native scrolling,
    // so they are replaced by arrows that drive the native scroll position.
    this.group.disablePagination = true;
    this.group.selectedTabChange.pipe(takeUntilDestroyed()).subscribe(event => {
      this.reveal(event.index);
    });
    this.group.focusChange.pipe(takeUntilDestroyed()).subscribe(event => {
      this.reveal(event.index);
    });

    afterNextRender(() => {
      const container = this.element.nativeElement.querySelector<HTMLElement>(
        ':scope > .mat-mdc-tab-header .mat-mdc-tab-label-container',
      );
      if (!container) return;
      this.container = container;

      this.zone.runOutsideAngular(() => {
        this.before = this.createArrow(container, -1);
        this.after = this.createArrow(container, 1);
        container.before(this.before);
        container.after(this.after);

        const update = () => this.updateArrows();
        container.addEventListener('scroll', update, { passive: true });
        let resize: ResizeObserver | undefined;
        if (typeof ResizeObserver !== 'undefined') {
          // Only refreshes arrow state; never changes the scroll position.
          resize = new ResizeObserver(update);
          resize.observe(container);
          const labels = container.querySelector<HTMLElement>('.mat-mdc-tab-labels');
          if (labels) resize.observe(labels);
        }

        this.destroyRef.onDestroy(() => {
          container.removeEventListener('scroll', update);
          resize?.disconnect();
          this.before?.remove();
          this.after?.remove();
        });

        update();
        // Only reveal on initial render and explicit selection/focus changes so manual scrolling is never overridden.
        this.reveal(this.group.selectedIndex ?? 0);
      });
    });

    this.destroyRef.onDestroy(() => {
      if (this.frame !== undefined) cancelAnimationFrame(this.frame);
    });
  }

  private createArrow(container: HTMLElement, direction: ScrollDirection): HTMLElement {
    const arrow = document.createElement('div');
    arrow.className = `app-tab-scroll-arrow mat-mdc-tab-header-pagination mat-mdc-tab-header-pagination-${
      direction < 0 ? 'before' : 'after'
    }`;
    arrow.setAttribute('aria-hidden', 'true');
    const chevron = document.createElement('div');
    chevron.className = 'mat-mdc-tab-header-pagination-chevron';
    arrow.appendChild(chevron);
    arrow.addEventListener('click', () => {
      container.scrollBy({ left: direction * container.clientWidth * 0.8, behavior: 'smooth' });
    });
    return arrow;
  }

  private updateArrows(): void {
    const container = this.container;
    if (!container || !this.before || !this.after) return;
    const max = container.scrollWidth - container.clientWidth;
    const overflowing = max > 1;
    this.element.nativeElement.classList.toggle('app-scrollable-tabs-overflowing', overflowing);
    this.before.classList.toggle('mat-mdc-tab-header-pagination-disabled', !overflowing || container.scrollLeft <= 1);
    this.after.classList.toggle('mat-mdc-tab-header-pagination-disabled', !overflowing || container.scrollLeft >= max - 1);
  }

  private reveal(index: number): void {
    const container = this.container;
    if (!container) return;
    if (this.frame !== undefined) cancelAnimationFrame(this.frame);

    // Material resets scrollLeft after keyboard focus; reveal once that handler has finished.
    this.frame = requestAnimationFrame(() => {
      this.frame = undefined;
      const tab = container.querySelectorAll<HTMLElement>('[role="tab"]')[index];
      if (!tab) return;

      const bounds = container.getBoundingClientRect();
      const label = tab.getBoundingClientRect();
      const left = bounds.left + container.clientLeft;
      const right = left + container.clientWidth;
      if (label.left < left) {
        container.scrollLeft += label.left - left;
      } else if (label.right > right) {
        container.scrollLeft += Math.min(label.right - right, label.left - left);
      }
    });
  }
}
