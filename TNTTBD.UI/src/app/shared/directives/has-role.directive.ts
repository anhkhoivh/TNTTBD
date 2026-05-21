import { Directive, Input, TemplateRef, ViewContainerRef, inject, effect } from '@angular/core';
import { AuthService } from '../../core/auth/auth.service';
import { UserRole } from '../enums';

@Directive({
  selector: '[hasRole]',
  standalone: true,
})
export class HasRoleDirective {
  private tpl = inject(TemplateRef<unknown>);
  private vcr = inject(ViewContainerRef);
  private auth = inject(AuthService);

  private roles: UserRole[] = [];

  @Input({ required: true }) set hasRole(value: UserRole[] | UserRole) {
    this.roles = Array.isArray(value) ? value : [value];
    this.render();
  }

  constructor() {
    effect(() => {
      this.auth.currentUser();
      this.render();
    });
  }

  private render(): void {
    this.vcr.clear();
    if (this.auth.hasRole(this.roles)) {
      this.vcr.createEmbeddedView(this.tpl);
    }
  }
}
