import { FullName, IsoDate, IsoDateTime } from './common.model';
import { AttendanceStatus, EnrollmentStatus, CatechismClassCatechistRole } from '../enums';

export interface CatechismYear {
  id: string;
  title: string;
  startDate: IsoDate;
  endDate: IsoDate;
  theme?: string | null;
}

export interface CatechismClass {
  id: string;
  catechismYearId: string;
  className: string;
  classCode: string;
  catechismYearTitle: string;
}

export interface CatechismSession {
  id: string;
  catechismClassId: string;
  dateTime: IsoDateTime;
  title: string;
}

export interface CatechismSessionAttendance {
  id?: string | null;
  catechismClassEucharisticYouthId: string;
  youthCode?: string | null;
  youthFullName?: FullName | null;
  attendanceStatus: AttendanceStatus;
}

export interface CatechismClassEucharisticYouth {
  id: string;
  code: string;
  catechismClassId: string;
  eucharisticYouthId: string;
  fullName: FullName;
  enrolledAt: IsoDateTime;
  leftAt?: IsoDateTime | null;
  status: EnrollmentStatus;
  note?: string | null;
}

export interface CatechismClassCatechist {
  id: string;
  catechismClassId: string;
  catechistId: string;
  role: CatechismClassCatechistRole;
  enrolledAt: IsoDateTime;
  leftAt?: IsoDateTime | null;
  status: EnrollmentStatus;
  note?: string | null;
}
