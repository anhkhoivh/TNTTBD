import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { debounceTime, of, switchMap } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { ApiService } from '../../core/services/api.service';
import { Catechist, CatechismClassCatechist, PaginatedResult } from '../models';
import { CatechismClassCatechistRole } from '../enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../pipes/full-name.pipe';

export interface CatechistAssignData {
  classId: string;
  existingAssignments: CatechismClassCatechist[];
}

@Component({
  selector: 'app-catechist-assign-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatButtonModule,
    TranslatePipe,
  ],
  templateUrl: './catechist-assign-dialog.html',
  styleUrl: './catechist-assign-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CatechistAssignDialog {
  protected ref = inject(MatDialogRef<CatechistAssignDialog>);
  protected data = inject<CatechistAssignData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private fullNamePipe = new FullNamePipe();

  protected readonly roles: { v: CatechismClassCatechistRole; k: string }[] = [
    { v: CatechismClassCatechistRole.Main, k: 'catechistRole.Main' },
    { v: CatechismClassCatechistRole.Member, k: 'catechistRole.Member' },
    { v: CatechismClassCatechistRole.Trainee, k: 'catechistRole.Trainee' },
    { v: CatechismClassCatechistRole.Auxiliary, k: 'catechistRole.Auxiliary' },
  ];

  protected hasMain = this.data.existingAssignments.some(
    (a) => a.role === CatechismClassCatechistRole.Main,
  );

  protected form = this.fb.nonNullable.group({
    catechistSearch: [''],
    role: [CatechismClassCatechistRole.Member as CatechismClassCatechistRole, Validators.required],
    note: [''],
  });

  protected selectedCatechist = signal<{ id: string; label: string } | null>(null);

  protected suggestions = toSignal(
    this.form.controls.catechistSearch.valueChanges.pipe(
      debounceTime(200),
      switchMap((search) => {
        const s = (search ?? '').trim();
        if (s.length < 1) return of([]);
        return this.api
          .list<Catechist>('/catechists', { page: 1, pageSize: 20 })
          .pipe(
            switchMap((r: PaginatedResult<Catechist>) => {
              const alreadyAssigned = new Set(this.data.existingAssignments.map((a) => a.catechistId));
              return of(
                r.items
                  .filter((c) => !alreadyAssigned.has(c.id))
                  .filter((c) => {
                    const name = this.fullNamePipe.transform(c.fullName).toLowerCase();
                    return name.includes(s.toLowerCase());
                  })
                  .map((c) => ({
                    id: c.id,
                    label: this.fullNamePipe.transform(c.fullName),
                  })),
              );
            }),
          );
      }),
    ),
    { initialValue: [] as { id: string; label: string }[] },
  );

  protected pickSuggestion(item: { id: string; label: string }): void {
    this.selectedCatechist.set(item);
    this.form.controls.catechistSearch.setValue(item.label, { emitEvent: false });
  }

  protected save(): void {
    const catechist = this.selectedCatechist();
    if (!catechist || this.form.invalid) return;

    const v = this.form.getRawValue();
    this.ref.close({
      catechismClassId: this.data.classId,
      catechistId: catechist.id,
      role: v.role,
      note: v.note || null,
    });
  }
}
