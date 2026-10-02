import { AttendanceStatus } from '../enums';

export interface AttendanceStatusDisplay {
  icon: string;
  color: string;
  labelKey: string;
  rank: number;
}

export const ATTENDANCE_STATUS_DISPLAY: Record<AttendanceStatus, AttendanceStatusDisplay> = {
  [AttendanceStatus.Present]: { icon: 'check_circle', color: '#81C784', labelKey: 'attendance.present', rank: 3 },
  [AttendanceStatus.Excused]: { icon: 'event_busy', color: 'orange', labelKey: 'attendance.excused', rank: 2 },
  [AttendanceStatus.Absent]: { icon: 'cancel', color: '#f66271', labelKey: 'attendance.absent', rank: 1 },
  [AttendanceStatus.None]: { icon: 'remove', color: '', labelKey: 'attendance.none', rank: 0 },
};
