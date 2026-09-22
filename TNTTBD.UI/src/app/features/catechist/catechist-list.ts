import { Component, inject , ChangeDetectionStrategy, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { AuthService } from '../../core/auth/auth.service';
import { MatListModule } from '@angular/material/list';
import { CatechistService } from './catechist.service';

@Component({
  selector: 'app-catechist-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
    MatListModule,
  ],
  templateUrl: './catechist-list.html',
  styleUrl: './catechist-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CatechistService],
})
export class CatechistList {
  private catechistService = inject(CatechistService);
  private auth = inject(AuthService);
  protected data = this.catechistService.catechists;
  protected cols = ['index', 'name', 'dob', 'gender'];
  private user = signal(this.auth.currentUser());
  protected myCatechistId = signal<string | null>(null);
  protected isMe(id: string): boolean {
    return this.user()?.catechistId != null && this.user()?.catechistId === id;
  }
}
