import { UserRole } from '../enums';

export interface UserExternalLogin {
  id: string;
  provider: string;
  subjectId: string;
  linkedAt: string;
}

export interface User {
  id: string;
  email: string;
  userName: string;
  isActive: boolean;
  userRole: UserRole;
  catechistId?: string | null;
  eucharisticYouthId?: string | null;
  contactId?: string | null;
  externalLogins?: UserExternalLogin[];
}
