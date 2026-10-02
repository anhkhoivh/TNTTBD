import { AttendanceStatus, EnrollmentStatus } from '../enums';
import { FullName, IsoDate, IsoDateTime } from './common.model';

export interface YouthReport {
  youthId: string;
  youthCode?: string | null;
  youthFullName?: FullName | null;
  catechismYearId?: string | null;
  selectedEnrollmentId?: string | null;
  enrollments: YouthClassReport[];
  attendance: YouthAttendanceSummary;
  exams: YouthExamSummary;
  attendanceDetails: YouthAttendanceReportItem[];
  examDetails: YouthExamReportItem[];
}

export interface YouthClassReport {
  enrollmentId: string;
  classId: string;
  classCode?: string | null;
  className?: string | null;
  catechismYearId: string;
  catechismYearName?: string | null;
  enrollmentStatus: EnrollmentStatus;
  enrolledAt: IsoDateTime;
  leftAt?: IsoDateTime | null;
  note?: string | null;
}

export interface YouthAttendanceSummary {
  sessionsHeld: number;
  present: number;
  absent: number;
  excused: number;
  unmarked: number;
  attendanceRate?: number | null;
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
  isCounted: boolean;
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
  sessionsHeld: number;
  sessions: ClassReportSession[];
  exams: ClassReportExam[];
  rows: ClassReportRow[];
}

export interface ClassReportSession {
  sessionId: string;
  title?: string | null;
  dateTime: IsoDateTime;
  isHeld: boolean;
}

export interface ClassReportExam {
  examId: string;
  title?: string | null;
  examDate: IsoDate;
}

export interface ClassReportAttendanceCell {
  sessionId: string;
  status: AttendanceStatus;
  isCounted: boolean;
}

export interface ClassReportScoreCell {
  examId: string;
  score?: number | null;
  remark?: string | null;
}

export interface ClassReportRow {
  enrollmentId: string;
  youthId: string;
  youthCode?: string | null;
  youthFullName?: FullName | null;
  enrollmentStatus: EnrollmentStatus;
  enrolledAt: IsoDateTime;
  leftAt?: IsoDateTime | null;
  attendances: ClassReportAttendanceCell[];
  scores: ClassReportScoreCell[];
  counted: number;
  present: number;
  absent: number;
  excused: number;
  unmarked: number;
  attendanceRate?: number | null;
  scored: number;
  averageScore?: number | null;
}
