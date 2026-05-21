import { Pipe, PipeTransform, inject } from '@angular/core';
import { TranslationService } from './translation.service';

@Pipe({
  name: 't',
  standalone: true,
  pure: false,
})
export class TranslatePipe implements PipeTransform {
  private svc = inject(TranslationService);

  transform(key: string): string {
    // Read the lang signal so this pipe updates when language changes
    this.svc.currentLang();
    return this.svc.t(key);
  }
}
