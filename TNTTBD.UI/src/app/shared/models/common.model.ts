export interface FullName {
  givenName: string;
  christName?: string | null;
  familyName?: string | null;
  middleName?: string | null;
}

export interface PatronSaint {
  saintName: string;
  day: number;
  month: number;
}

export interface PaginatedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

/** ISO 8601 date string with offset, e.g. "2026-05-21T19:30:00+07:00" */
export type IsoDateTime = string;

/** Calendar-only date string, "YYYY-MM-DD" */
export type IsoDate = string;
