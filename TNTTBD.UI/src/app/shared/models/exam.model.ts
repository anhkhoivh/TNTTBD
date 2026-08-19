import { FullName, IsoDate } from './common.model';

export interface ExamResult {
  id?: string | null;
  catechismClassEucharisticYouthId: string;
  youthCode?: string | null;
  youthFullName?: FullName | null;
  score: number | null;
  remark?: string | null;
}

export interface Exam {
  id: string;
  catechismClassId: string;
  examDate: IsoDate;
  title: string;
  examResults: ExamResult[];
}
