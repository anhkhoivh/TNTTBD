import { Component, inject, signal , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
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
import { ThemeService } from '../services/theme.service';
import { LoadingService } from '../services/loading.service';
import { TranslationService, Lang } from '../i18n/translation.service';
import { TranslatePipe } from '../i18n/translate.pipe';
import { AuthService } from '../auth/auth.service';
import { UserRole } from '../../shared/enums';

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
    TranslatePipe,
  ],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Shell {
  private breakpoint = inject(BreakpointObserver);
  protected theme = inject(ThemeService);
  protected loading = inject(LoadingService);
  protected translation = inject(TranslationService);
  protected auth = inject(AuthService);

  protected readonly UserRole = UserRole;

  protected isHandset = toSignal(
    this.breakpoint.observe([Breakpoints.Handset, Breakpoints.Small]).pipe(map((r) => r.matches)),
    { initialValue: false },
  );

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
      roles: [UserRole.Admin],
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

  protected toggleSidenav(): void {
    this.sidenavOpened.update((v) => !v);
  }

  protected closeOnHandset(): void {
    if (this.isHandset()) this.sidenavOpened.set(false);
  }
}
