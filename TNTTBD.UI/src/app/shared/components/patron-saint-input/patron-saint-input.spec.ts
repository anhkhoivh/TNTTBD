import { ChangeDetectionStrategy, Component, effect, signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { TranslationService } from '../../../core/i18n/translation.service';
import { PatronSaint } from '../../models';
import { PatronSaintInput } from './patron-saint-input';

@Component({
  standalone: true,
  imports: [ReactiveFormsModule, PatronSaintInput],
  template: `<form [formGroup]="form"><app-patron-saint-input formControlName="patronSaints" /></form>`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
class HostForm {
  readonly detail = signal<PatronSaint[] | null>(null);
  readonly form = new FormBuilder().group({ patronSaints: [[] as PatronSaint[]] });

  constructor() {
    // Deliberately tracked: the control must not make the parent effect depend on its internal state.
    effect(() => {
      const saints = this.detail();
      if (saints) this.form.patchValue({ patronSaints: saints });
    });
  }
}

describe('PatronSaintInput', () => {
  const existing: PatronSaint = { saintName: 'Giuse', day: 19, month: 3 };

  function create(saints: PatronSaint[] | null = null) {
    TestBed.configureTestingModule({
      imports: [HostForm],
      providers: [
        provideNoopAnimations(),
        { provide: TranslationService, useValue: { currentLang: signal('en'), t: (key: string) => key } },
      ],
    });
    const fixture = TestBed.createComponent(HostForm);
    fixture.detectChanges();
    // Load after first render, like the detail resource does, so the effect calls writeValue.
    fixture.componentInstance.detail.set(saints);
    TestBed.tick();
    fixture.detectChanges();
    return fixture;
  }

  const rows = (fixture: ReturnType<typeof create>) =>
    fixture.nativeElement.querySelectorAll('.patron-row') as NodeListOf<HTMLElement>;
  const addButton = (fixture: ReturnType<typeof create>) =>
    Array.from(fixture.nativeElement.querySelectorAll('button') as NodeListOf<HTMLButtonElement>)
      .find(button => button.textContent?.includes('patron.add'))!;

  function type(input: HTMLInputElement, value: string) {
    input.value = value;
    input.dispatchEvent(new Event('input'));
  }

  it('makes the form valid after naming a newly added saint', () => {
    const fixture = create([]);
    addButton(fixture).click();
    TestBed.tick();
    fixture.detectChanges();
    expect(fixture.componentInstance.form.invalid).toBe(true);

    type(rows(fixture)[0].querySelector('input')!, 'Maria');
    fixture.detectChanges();

    expect(fixture.componentInstance.form.valid).toBe(true);
    expect(fixture.componentInstance.form.value.patronSaints).toEqual([{ saintName: 'Maria', day: 1, month: 1 }]);
  });

  it('keeps existing saints when adding another', () => {
    const fixture = create([existing]);
    expect(rows(fixture).length).toBe(1);

    addButton(fixture).click();
    TestBed.tick();
    fixture.detectChanges();

    expect(rows(fixture).length).toBe(2);
    expect(fixture.componentInstance.form.value.patronSaints).toEqual([existing, { saintName: '', day: 1, month: 1 }]);
  });

  it('propagates edits to existing saints', () => {
    const fixture = create([existing]);
    type(rows(fixture)[0].querySelector('input')!, 'Giuse Thợ');
    fixture.detectChanges();
    expect(fixture.componentInstance.form.value.patronSaints).toEqual([{ ...existing, saintName: 'Giuse Thợ' }]);
  });

  it('loads without marking the form dirty and clears rows for an empty list', () => {
    const fixture = create([existing]);
    expect(fixture.componentInstance.form.dirty).toBe(false);

    fixture.componentInstance.form.patchValue({ patronSaints: [] });
    fixture.detectChanges();

    expect(rows(fixture).length).toBe(0);
    expect(fixture.componentInstance.form.valid).toBe(true);
  });
});
