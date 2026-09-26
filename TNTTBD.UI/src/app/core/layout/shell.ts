import { Component, inject, signal , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs/operators';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ThemeService } from '../services/theme.service';
import { LoadingService } from '../services/loading.service';
import { TranslationService, Lang } from '../i18n/translation.service';
import { TranslatePipe } from '../i18n/translate.pipe';
import { AuthService } from '../auth/auth.service';
import { UserRole } from '../../shared/enums';
import { ChangePasswordDialog } from '../../shared/components/change-password-dialog';

interface NavItem {
  labelKey: string;
  route: string;
  icon: string;
  roles?: UserRole[];
}

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatMenuModule,
    MatDividerModule,
    MatProgressBarModule,
    MatDialogModule,
    TranslatePipe,
  ],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Shell {
  private dialog = inject(MatDialog);
  protected theme = inject(ThemeService);
  protected loading = inject(LoadingService);
  protected translation = inject(TranslationService);
  protected auth = inject(AuthService);
  protected router = inject(Router);

  protected readonly UserRole = UserRole;

  protected sidenavOpened = signal(false);

  protected navItems: NavItem[] = [
    { labelKey: 'nav.dashboard', route: '/app/dashboard', icon: 'dashboard' },
    {
      labelKey: 'nav.youth',
      route: '/app/youth',
      icon: 'groups',
      roles: [UserRole.Admin, UserRole.Catechist],
    },
    {
      labelKey: 'nav.catechists',
      route: '/app/catechists',
      icon: 'school',
      roles: [UserRole.Admin, UserRole.Catechist],
    },
    {
      labelKey: 'nav.contacts',
      route: '/app/contacts',
      icon: 'contacts',
      roles: [UserRole.Admin, UserRole.Catechist],
    },
    {
      labelKey: 'nav.classes',
      route: '/app/classes',
      icon: 'class',
      roles: [UserRole.Admin, UserRole.Catechist, UserRole.EucharisticYouth, UserRole.EucharisticYouthContact],
    },
    {
      labelKey: 'nav.admin',
      route: '/app/admin/users',
      icon: 'admin_panel_settings',
      roles: [UserRole.Admin],
    },
  ];

  protected visibleNav(): NavItem[] {
    return this.navItems.filter(
      (item) => !item.roles || this.auth.hasRole(item.roles),
    );
  }

  protected toggleTheme(): void {
    this.theme.toggle();
  }

  protected async setLang(lang: Lang): Promise<void> {
    await this.translation.setLang(lang);
  }

  protected logout(): void {
    this.auth.logout();
  }

  protected changePassword(): void {
    this.dialog.open(ChangePasswordDialog);
  }

  protected toggleSidenav(): void {
    this.sidenavOpened.update((v) => !v);
  }

  protected viewProfile(): void {
    if (!this.auth.currentUser()) {
      return;
    }
    if (this.auth.currentUser() && this.auth.isAdminOrCatechist() && this.auth.currentUser()?.catechistId) {
      this.router.navigate(['/app/catechists/detail'], { queryParams: { id:  this.auth.currentUser()?.catechistId} });
    }
    else if (this.auth.currentUser() && this.auth.currentUser()?.eucharisticYouthId) {
      this.router.navigate(['/app/youth/detail'], { queryParams: { id:  this.auth.currentUser()?.eucharisticYouthId} });
    }
    else if (this.auth.currentUser()?.contactId) {
      this.router.navigate(['/app/contacts/detail'], { queryParams: { id: this.auth.currentUser()?.contactId } });
    }
  }
}
