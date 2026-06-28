import { Pipe, PipeTransform } from '@angular/core';
import { FullName } from '../models';

@Pipe({
  name: 'fullName',
  standalone: true,
})
export class FullNamePipe implements PipeTransform {
  transform(value: FullName | null | undefined): string {
    if (!value) return '';
    // Vietnamese display order: ChristName FamilyName MiddleName GivenName
    return [value.christName, value.familyName, value.middleName, value.givenName]
      .filter((p) => !!p && p.trim().length > 0)
      .join(' ');
  }
}
