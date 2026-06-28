import { IsoDate, IsoDateTime } from './common.model';

export interface ExamResult {
  id: string;
  examId: string;
  catechismClassEucharisticYouthId: string;
  markedByCatechistId: string;
  markedTime: IsoDateTime;
  score: number;
  isDone: boolean;
  remark?: string | null;
}

export interface Exam {
  id: string;
  catechismClassId: string;
  examDate: IsoDate;
  title: string;
  examResults: ExamResult[];
}
