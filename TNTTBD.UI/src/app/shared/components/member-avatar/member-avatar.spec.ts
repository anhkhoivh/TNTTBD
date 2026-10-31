import { signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { Subject, of } from 'rxjs';
import { PhotoService } from '../../../core/services/photo.service';
import { MemberAvatar } from './member-avatar';

describe('MemberAvatar', () => {
  function create(source: ReturnType<PhotoService['profilePhoto']>, entityType: 'youths' | 'catechists' = 'youths') {
    const changes = signal(0);
    const calls: string[] = [];
    TestBed.configureTestingModule({
      imports: [MemberAvatar],
      providers: [
        {
          provide: PhotoService,
          useValue: {
            profileChanges: changes.asReadonly(),
            profilePhoto: (type: string, id: string) => {
              calls.push(`${type}:${id}`);
              return source;
            },
          },
        },
      ],
    });
    const fixture = TestBed.createComponent(MemberAvatar);
    fixture.componentRef.setInput('entityType', entityType);
    fixture.componentRef.setInput('entityId', 'abc');
    fixture.componentRef.setInput('alt', 'Maria');
    fixture.detectChanges();
    TestBed.tick();
    fixture.detectChanges();
    return { fixture, changes, calls };
  }

  it('shows the profile photo when one is available', () => {
    const { fixture, calls } = create(of('blob:photo'));
    const img = fixture.nativeElement.querySelector('img') as HTMLImageElement;
    expect(img.getAttribute('src')).toBe('blob:photo');
    expect(fixture.nativeElement.querySelector('mat-icon')).toBeNull();
    expect(fixture.nativeElement.getAttribute('aria-label')).toBe('Maria');
    expect(calls).toEqual(['youths:abc']);
  });

  it('falls back to a generic icon when there is no photo', () => {
    const { fixture } = create(of(null), 'catechists');
    expect(fixture.nativeElement.querySelector('img')).toBeNull();
    expect(fixture.nativeElement.querySelector('mat-icon').textContent.trim()).toBe('person');
  });

  it('shows the icon while loading and reloads after an invalidation', () => {
    const pending = new Subject<string | null>();
    const { fixture, changes, calls } = create(pending);
    expect(fixture.nativeElement.querySelector('mat-icon').textContent.trim()).toBe('face');

    pending.next('blob:first');
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('img')).not.toBeNull();

    changes.update((n) => n + 1);
    TestBed.tick();
    fixture.detectChanges();
    expect(calls.length).toBe(2);
  });
});
