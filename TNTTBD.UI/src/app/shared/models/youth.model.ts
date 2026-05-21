import { FullName, IsoDate, IsoDateTime, PatronSaint } from './common.model';
import { MilestoneType, PhotoPurpose, PhotoVisibility } from '../enums';

export interface PhotoReference {
  id: string;
  storageKey: string;
  contentType: string;
  purpose: PhotoPurpose;
  uploadedAt: IsoDateTime;
  uploadedBy: string;
  visibility: PhotoVisibility;
}

export interface SpiritualMilestone {
  id: string;
  type: MilestoneType;
  date: IsoDate;
  documentCode?: string | null;
  place?: string | null;
  note?: string | null;
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
