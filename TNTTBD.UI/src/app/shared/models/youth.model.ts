import { FullName, IsoDate, PatronSaint } from './common.model';
import { MilestoneType, PhotoPurpose } from '../enums';

export interface PhotoReference {
  photoId: string;
  url: string;
  contentType: string;
  purpose: PhotoPurpose;
  uploadedBy: string;
}

export interface SpiritualMilestone {
  id: string;
  type: MilestoneType;
  date: IsoDate;
  documentCode?: string | null;
  place?: string | null;
  note?: string | null;
  sponsorContactId?: string | null;
  celebrant?: string | null;
  milestoneEventId?: string | null;
  milestoneEventName?: string | null;
  /** Read-only, resolved from the sponsor contact. */
  sponsorFullName?: FullName | null;
  sponsorPhoneNumber?: string | null;
}

export interface EucharisticYouth {
  id: string;
  code: string;
  fullName: FullName;
  dateOfBirth: IsoDate;
  address?: string | null;
  isMale: boolean;
  photos: PhotoReference[];
  patronSaints: PatronSaint[];
  milestones: SpiritualMilestone[];
}
