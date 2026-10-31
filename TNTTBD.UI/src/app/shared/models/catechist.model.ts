import { FullName, IsoDate, PatronSaint } from './common.model';
import { PhotoReference } from './youth.model';

export interface Catechist {
  id: string;
  fullName: FullName;
  dateOfBirth: IsoDate;
  address?: string | null;
  phoneNumber?: string | null;
  isMale: boolean;
  photos: PhotoReference[];
  patronSaints: PatronSaint[];
}
