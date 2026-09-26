import { FullName } from './common.model';
import { ContactRelationType } from '../enums';

export interface Contact {
  id: string;
  fullName: FullName;
  phoneNumber?: string | null;
  email?: string | null;
}

export interface EucharisticYouthContact {
  id: string;
  eucharisticYouthId: string;
  contactId: string;
  contactRelationType: ContactRelationType;
  contact?: Contact;
}

export interface ContactYouth {
  /** Link id (EucharisticYouthContact) */
  id: string;
  eucharisticYouthId: string;
  code: string;
  fullName: FullName;
  contactRelationType: ContactRelationType;
}

export interface ContactDetail extends Contact {
  youths: ContactYouth[];
}
