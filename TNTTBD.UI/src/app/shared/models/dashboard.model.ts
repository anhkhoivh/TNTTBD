import { IsoDateTime } from './common.model';

export interface DashboardClass {
  id: string;
  className: string;
}

export interface DashboardSession {
  id: string;
  title: string;
  dateTime: IsoDateTime;
  catechismClassId: string;
  className: string;
}

export interface DashboardTotals {
  youth: number;
  catechists: number;
  classes: number;
  years: number;
}

export interface DashboardSummary {
  currentYearTitle: string;
  myClasses: DashboardClass[];
  upcomingSessions: DashboardSession[];
  totals?: DashboardTotals | null;
}

export interface CatechistCurrentClass {
  classId: string;
  className: string;
  catechismYearId: string;
  catechismYearTitle: string;
}
