import { AttendanceStatus, EnrollmentStatus } from '../enums';
import { FullName, IsoDate, IsoDateTime, PaginatedResult } from './common.model';

export interface YouthReport {
  youthId: string;
  youthCode?: string | null;
  youthFullName?: FullName | null;
  attendance: YouthAttendanceSummary;
  exams: YouthExamSummary;
  classes: YouthClassReport[];
  attendanceDetails: PaginatedResult<YouthAttendanceReportItem>;
  examDetails: PaginatedResult<YouthExamReportItem>;
}

export interface YouthClassReport {
  classId: string;
  classCode?: string | null;
  className?: string | null;
  catechismYearId: string;
  catechismYearName?: string | null;
  enrollmentStatus: EnrollmentStatus;
  enrolledAt: IsoDateTime;
  leftAt?: IsoDateTime | null;
}

export interface YouthAttendanceSummary {
  total: number;
  present: number;
  absent: number;
  excused: number;
  attendanceRate: number;
}

export interface YouthExamSummary {
  total: number;
  scored: number;
  averageScore?: number | null;
}

export interface YouthAttendanceReportItem {
  sessionId: string;
  sessionTitle?: string | null;
  sessionDateTime: IsoDateTime;
  classId: string;
  className?: string | null;
  status: AttendanceStatus;
}

export interface YouthExamReportItem {
  examId: string;
  examTitle?: string | null;
  examDate: IsoDate;
  classId: string;
  className?: string | null;
  score?: number | null;
  remark?: string | null;
}

export interface ClassReport {
  classId: string;
  classCode?: string | null;
  className?: string | null;
  catechismYearId: string;
  catechismYearName?: string | null;
  sessions: ClassSessionReportItem[];
  exams: ClassExamReportItem[];
}

export interface ClassSessionReportItem {
  sessionId: string;
  title?: string | null;
  dateTime: IsoDateTime;
  totalAttendance: number;
  present: number;
  absent: number;
  excused: number;
}

export interface ClassExamReportItem {
  examId: string;
  title?: string | null;
  examDate: IsoDate;
  totalResults: number;
  scored: number;
  averageScore?: number | null;
}