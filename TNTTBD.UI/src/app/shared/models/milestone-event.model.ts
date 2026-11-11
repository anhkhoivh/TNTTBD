import { FullName, IsoDate } from './common.model';
import { MilestoneType } from '../enums';

export interface MilestoneEventSummary {
  id: string;
  name: string;
  type: MilestoneType;
  date: IsoDate;
  place?: string | null;
  participantCount: number;
}

export interface MilestoneSponsor {
  contactId: string;
  fullName: FullName;
  phoneNumber?: string | null;
}

export interface MilestonePrerequisiteWarning {
  /** The prerequisite milestone that is missing or dated after this one. */
  type: MilestoneType;
  issue: 'Missing' | 'DatedAfter';
}

export interface MilestoneEventParticipant {
  milestoneId: string;
  eucharisticYouthId: string;
  youthCode: string;
  fullName: FullName;
  dateOfBirth?: IsoDate | null;
  className?: string | null;
  date?: IsoDate | null;
  place?: string | null;
  documentCode?: string | null;
  celebrant?: string | null;
  sponsor?: MilestoneSponsor | null;
  note?: string | null;
  baptismDate?: IsoDate | null;
  warnings: MilestonePrerequisiteWarning[];
}

export interface MilestoneEventDetail {
  id: string;
  name: string;
  type: MilestoneType;
  date: IsoDate;
  place?: string | null;
  celebrant?: string | null;
  note?: string | null;
  codeTemplate?: string | null;
  startIndex: number;
  participants: MilestoneEventParticipant[];
}

export interface MilestoneEventCandidateMilestone {
  milestoneId: string;
  date?: IsoDate | null;
  place?: string | null;
  documentCode?: string | null;
  celebrant?: string | null;
  sponsor?: MilestoneSponsor | null;
  note?: string | null;
  milestoneEventId?: string | null;
  milestoneEventName?: string | null;
}

export interface MilestoneEventCandidate {
  eucharisticYouthId: string;
  youthCode: string;
  fullName: FullName;
  dateOfBirth?: IsoDate | null;
  className?: string | null;
  hasBaptism: boolean;
  baptismDate?: IsoDate | null;
  existingMilestone?: MilestoneEventCandidateMilestone | null;
  warnings: MilestonePrerequisiteWarning[];
}

export interface SaveMilestoneEventParticipant {
  eucharisticYouthId: string;
  milestoneId?: string | null;
  date?: IsoDate | null;
  place?: string | null;
  documentCode?: string | null;
  celebrant?: string | null;
  sponsorContactId?: string | null;
  note?: string | null;
}

export interface SaveMilestoneEventPayload {
  id?: string | null;
  name: string;
  type: MilestoneType;
  date: IsoDate;
  place?: string | null;
  celebrant?: string | null;
  note?: string | null;
  codeTemplate?: string | null;
  startIndex: number;
  participants: SaveMilestoneEventParticipant[];
}
