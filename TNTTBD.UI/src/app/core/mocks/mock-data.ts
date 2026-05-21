import { User } from '../../shared/models';
import {
  UserRole,
  MilestoneType,
  EnrollmentStatus,
  AttendanceStatus,
  CatechismClassCatechistRole,
  ContactRelationType,
  PhotoPurpose,
  PhotoVisibility,
} from '../../shared/enums';
import {
  EucharisticYouth,
  Catechist,
  Contact,
  EucharisticYouthContact,
  CatechismYear,
  CatechismClass,
  CatechismSession,
  CatechismSessionAttendance,
  CatechismClassEucharisticYouth,
  CatechismClassCatechist,
  Exam,
} from '../../shared/models';

const photo = (seed: string) => ({
  id: `p-${seed}`,
  storageKey: `https://picsum.photos/seed/${seed}/400/400`,
  contentType: 'image/jpeg',
  purpose: PhotoPurpose.Profile,
  uploadedAt: '2025-09-01T08:00:00+07:00',
  uploadedBy: 'u-admin',
  visibility: PhotoVisibility.Public,
});

export const mockUsers: User[] = [
  { id: 'u-admin', email: 'admin@tnttbd.local', userName: 'admin', isActive: true, userRole: UserRole.Admin },
  { id: 'u-cat1', email: 'catechist1@tnttbd.local', userName: 'catechist1', isActive: true, userRole: UserRole.Catechist, catechistId: 'c-1' },
  { id: 'u-cat2', email: 'catechist2@tnttbd.local', userName: 'catechist2', isActive: true, userRole: UserRole.Catechist, catechistId: 'c-2' },
  { id: 'u-cat3', email: 'catechist3@tnttbd.local', userName: 'catechist3', isActive: false, userRole: UserRole.Catechist, catechistId: 'c-3' },
  { id: 'u-contact1', email: 'parent1@tnttbd.local', userName: 'parent1', isActive: true, userRole: UserRole.EucharisticYouthContact, contactId: 'co-1' },
  { id: 'u-contact2', email: 'parent2@tnttbd.local', userName: 'parent2', isActive: true, userRole: UserRole.EucharisticYouthContact, contactId: 'co-2' },
  { id: 'u-youth1', email: 'youth1@tnttbd.local', userName: 'youth1', isActive: true, userRole: UserRole.EucharisticYouth, eucharisticYouthId: 'y-1' },
];

export const mockCatechists: Catechist[] = [
  {
    id: 'c-1',
    fullName: { givenName: 'Anh', familyName: 'Nguyễn', christName: 'Phêrô' },
    dateOfBirth: '1995-03-12',
    isMale: true,
    address: 'Giáo xứ Bùi Đậu',
    photos: [photo('cat-1')],
    patronSaints: [{ saintName: 'Phêrô', day: 29, month: 6 }],
  },
  {
    id: 'c-2',
    fullName: { givenName: 'Mai', familyName: 'Trần', christName: 'Maria' },
    dateOfBirth: '1997-08-05',
    isMale: false,
    address: 'Giáo xứ Bùi Đậu',
    photos: [photo('cat-2')],
    patronSaints: [{ saintName: 'Maria', day: 15, month: 8 }],
  },
  {
    id: 'c-3',
    fullName: { givenName: 'Hoà', familyName: 'Vũ', christName: 'Gioan' },
    dateOfBirth: '1999-11-20',
    isMale: true,
    address: 'Giáo xứ Bùi Đậu',
    photos: [photo('cat-3')],
    patronSaints: [{ saintName: 'Gioan', day: 27, month: 12 }],
  },
];

const youthSeed = (
  id: string,
  code: string,
  given: string,
  family: string,
  christ: string,
  dob: string,
  isMale: boolean,
  milestones: { type: MilestoneType; date: string; place?: string }[],
): EucharisticYouth => ({
  id,
  code,
  fullName: { givenName: given, familyName: family, christName: christ },
  dateOfBirth: dob,
  isMale,
  address: 'Giáo xứ Bùi Đậu',
  photos: [photo(id)],
  patronSaints: [{ saintName: christ, day: 1, month: 1 }],
  milestones: milestones.map((m, i) => ({
    id: `m-${id}-${i}`,
    type: m.type,
    date: m.date,
    place: m.place ?? 'Giáo xứ Bùi Đậu',
  })),
});

export const mockYouth: EucharisticYouth[] = [
  youthSeed('y-1', 'TN001', 'An', 'Nguyễn', 'Giuse', '2014-05-21', true, [
    { type: MilestoneType.Baptism, date: '2014-08-15' },
    { type: MilestoneType.FirstCommunion, date: '2022-06-05' },
  ]),
  youthSeed('y-2', 'TN002', 'Bình', 'Lê', 'Maria', '2015-09-10', false, [
    { type: MilestoneType.Baptism, date: '2015-11-01' },
  ]),
  youthSeed('y-3', 'TN003', 'Cường', 'Phạm', 'Phaolô', '2013-02-14', true, [
    { type: MilestoneType.Baptism, date: '2013-04-10' },
    { type: MilestoneType.FirstPenance, date: '2021-03-15' },
    { type: MilestoneType.FirstCommunion, date: '2021-06-12' },
  ]),
  youthSeed('y-4', 'TN004', 'Dung', 'Hoàng', 'Têrêsa', '2012-07-08', false, [
    { type: MilestoneType.Baptism, date: '2012-09-20' },
    { type: MilestoneType.FirstCommunion, date: '2020-05-10' },
    { type: MilestoneType.Confirmation, date: '2024-04-20' },
  ]),
  youthSeed('y-5', 'TN005', 'Em', 'Đỗ', 'Anrê', '2014-12-01', true, [
    { type: MilestoneType.Baptism, date: '2015-02-14' },
  ]),
  youthSeed('y-6', 'TN006', 'Phú', 'Bùi', 'Giacôbê', '2013-06-18', true, [
    { type: MilestoneType.Baptism, date: '2013-08-22' },
    { type: MilestoneType.FirstCommunion, date: '2021-06-05' },
  ]),
  youthSeed('y-7', 'TN007', 'Giang', 'Ngô', 'Anna', '2015-03-25', false, [
    { type: MilestoneType.Baptism, date: '2015-05-30' },
  ]),
  youthSeed('y-8', 'TN008', 'Hùng', 'Trịnh', 'Tôma', '2011-10-11', true, [
    { type: MilestoneType.Baptism, date: '2012-01-08' },
    { type: MilestoneType.FirstCommunion, date: '2019-06-09' },
    { type: MilestoneType.Confirmation, date: '2023-05-21' },
  ]),
  youthSeed('y-9', 'TN009', 'Lan', 'Mai', 'Rosa', '2014-04-04', false, [
    { type: MilestoneType.Baptism, date: '2014-06-22' },
    { type: MilestoneType.FirstCommunion, date: '2022-06-05' },
  ]),
  youthSeed('y-10', 'TN010', 'Minh', 'Tô', 'Đa Minh', '2016-01-30', true, [
    { type: MilestoneType.Baptism, date: '2016-04-03' },
  ]),
];

export const mockContacts: Contact[] = [
  { id: 'co-1', fullName: { givenName: 'Hùng', familyName: 'Nguyễn' }, phoneNumber: '0901234567', email: 'parent1@tnttbd.local' },
  { id: 'co-2', fullName: { givenName: 'Lan', familyName: 'Lê' }, phoneNumber: '0907654321', email: 'parent2@tnttbd.local' },
  { id: 'co-3', fullName: { givenName: 'Tâm', familyName: 'Phạm' }, phoneNumber: '0912345678' },
  { id: 'co-4', fullName: { givenName: 'Hồng', familyName: 'Hoàng' }, phoneNumber: '0987654321' },
];

export const mockYouthContacts: EucharisticYouthContact[] = [
  { id: 'yc-1', eucharisticYouthId: 'y-1', contactId: 'co-1', contactRelationType: ContactRelationType.Father },
  { id: 'yc-2', eucharisticYouthId: 'y-2', contactId: 'co-2', contactRelationType: ContactRelationType.Mother },
  { id: 'yc-3', eucharisticYouthId: 'y-3', contactId: 'co-3', contactRelationType: ContactRelationType.Mother },
  { id: 'yc-4', eucharisticYouthId: 'y-4', contactId: 'co-4', contactRelationType: ContactRelationType.Father },
];

export const mockYears: CatechismYear[] = [
  { id: 'yr-2024', startDate: '2024-09-01', endDate: '2025-06-30', theme: 'Yêu thương và phục vụ' },
  { id: 'yr-2025', startDate: '2025-09-01', endDate: '2026-06-30', theme: 'Sống Lời Chúa' },
];

export const mockClasses: CatechismClass[] = [
  { id: 'cl-1', catechismYearId: 'yr-2025', className: 'Khai tâm 1' },
  { id: 'cl-2', catechismYearId: 'yr-2025', className: 'Rước lễ 1' },
  { id: 'cl-3', catechismYearId: 'yr-2025', className: 'Thêm sức 2' },
  { id: 'cl-4', catechismYearId: 'yr-2025', className: 'Bao đồng' },
  { id: 'cl-prev-1', catechismYearId: 'yr-2024', className: 'Khai tâm 1 (2024)' },
];

export const mockClassCatechists: CatechismClassCatechist[] = [
  { id: 'clc-1', catechismClassId: 'cl-1', catechistId: 'c-1', role: CatechismClassCatechistRole.Main },
  { id: 'clc-2', catechismClassId: 'cl-1', catechistId: 'c-2', role: CatechismClassCatechistRole.Member },
  { id: 'clc-3', catechismClassId: 'cl-2', catechistId: 'c-2', role: CatechismClassCatechistRole.Main },
  { id: 'clc-4', catechismClassId: 'cl-2', catechistId: 'c-3', role: CatechismClassCatechistRole.Assistance },
  { id: 'clc-5', catechismClassId: 'cl-3', catechistId: 'c-1', role: CatechismClassCatechistRole.Main },
];

export const mockClassYouth: CatechismClassEucharisticYouth[] = [
  { id: 'cly-1', catechismClassId: 'cl-1', eucharisticYouthId: 'y-1', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-2', catechismClassId: 'cl-1', eucharisticYouthId: 'y-2', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-3', catechismClassId: 'cl-1', eucharisticYouthId: 'y-5', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-4', catechismClassId: 'cl-1', eucharisticYouthId: 'y-7', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-5', catechismClassId: 'cl-1', eucharisticYouthId: 'y-10', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Pending },
  { id: 'cly-6', catechismClassId: 'cl-2', eucharisticYouthId: 'y-3', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-7', catechismClassId: 'cl-2', eucharisticYouthId: 'y-6', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-8', catechismClassId: 'cl-2', eucharisticYouthId: 'y-9', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-9', catechismClassId: 'cl-3', eucharisticYouthId: 'y-4', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-10', catechismClassId: 'cl-3', eucharisticYouthId: 'y-8', enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'cly-h1', catechismClassId: 'cl-prev-1', eucharisticYouthId: 'y-1', enrolledAt: '2024-09-01T08:00:00+07:00', leftAt: '2025-06-30T17:00:00+07:00', status: EnrollmentStatus.Completed },
  { id: 'cly-h2', catechismClassId: 'cl-prev-1', eucharisticYouthId: 'y-3', enrolledAt: '2024-09-01T08:00:00+07:00', leftAt: '2025-06-30T17:00:00+07:00', status: EnrollmentStatus.Completed },
];

export const mockSessions: CatechismSession[] = [
  { id: 's-1', catechismClassId: 'cl-1', dateTime: '2025-11-09T08:00:00+07:00', title: 'Buổi 1: Khởi đầu năm học' },
  { id: 's-2', catechismClassId: 'cl-1', dateTime: '2025-11-16T08:00:00+07:00', title: 'Buổi 2: Lời Chúa' },
  { id: 's-3', catechismClassId: 'cl-1', dateTime: '2025-11-23T08:00:00+07:00', title: 'Buổi 3: Bí tích Rửa tội' },
  { id: 's-4', catechismClassId: 'cl-1', dateTime: '2025-12-07T08:00:00+07:00', title: 'Buổi 4: Mùa Vọng' },
  { id: 's-5', catechismClassId: 'cl-2', dateTime: '2025-11-09T09:30:00+07:00', title: 'Buổi 1: Rước lễ là gì?' },
  { id: 's-6', catechismClassId: 'cl-2', dateTime: '2025-11-16T09:30:00+07:00', title: 'Buổi 2: Bữa Tiệc Ly' },
  { id: 's-7', catechismClassId: 'cl-3', dateTime: '2025-11-09T15:00:00+07:00', title: 'Buổi 1: Chúa Thánh Thần' },
];

export const mockAttendances: CatechismSessionAttendance[] = [
  { id: 'a-1', catechismSessionId: 's-1', catechismClassEucharisticYouthId: 'cly-1', attendanceStatus: AttendanceStatus.Present, markedTime: '2025-11-09T08:15:00+07:00', markedByCatechistId: 'c-1' },
  { id: 'a-2', catechismSessionId: 's-1', catechismClassEucharisticYouthId: 'cly-2', attendanceStatus: AttendanceStatus.Absent, markedTime: '2025-11-09T08:15:00+07:00', markedByCatechistId: 'c-1' },
  { id: 'a-3', catechismSessionId: 's-1', catechismClassEucharisticYouthId: 'cly-3', attendanceStatus: AttendanceStatus.Present, markedTime: '2025-11-09T08:15:00+07:00', markedByCatechistId: 'c-1' },
  { id: 'a-4', catechismSessionId: 's-1', catechismClassEucharisticYouthId: 'cly-4', attendanceStatus: AttendanceStatus.Excused, markedTime: '2025-11-09T08:15:00+07:00', markedByCatechistId: 'c-1' },
  { id: 'a-5', catechismSessionId: 's-2', catechismClassEucharisticYouthId: 'cly-1', attendanceStatus: AttendanceStatus.Present, markedTime: '2025-11-16T08:15:00+07:00', markedByCatechistId: 'c-1' },
  { id: 'a-6', catechismSessionId: 's-2', catechismClassEucharisticYouthId: 'cly-2', attendanceStatus: AttendanceStatus.Present, markedTime: '2025-11-16T08:15:00+07:00', markedByCatechistId: 'c-1' },
];

export const mockExams: Exam[] = [
  {
    id: 'ex-1',
    catechismClassId: 'cl-1',
    examDate: '2025-12-15',
    title: 'Kiểm tra giữa kỳ I',
    examResults: [
      { id: 'er-1', examId: 'ex-1', catechismClassEucharisticYouthId: 'cly-1', markedByCatechistId: 'c-1', markedTime: '2025-12-15T10:00:00+07:00', score: 8.5, isDone: true },
      { id: 'er-2', examId: 'ex-1', catechismClassEucharisticYouthId: 'cly-2', markedByCatechistId: 'c-1', markedTime: '2025-12-15T10:00:00+07:00', score: 7.0, isDone: true },
      { id: 'er-3', examId: 'ex-1', catechismClassEucharisticYouthId: 'cly-3', markedByCatechistId: 'c-1', markedTime: '2025-12-15T10:00:00+07:00', score: 9.0, isDone: true },
    ],
  },
  {
    id: 'ex-2',
    catechismClassId: 'cl-1',
    examDate: '2026-04-20',
    title: 'Kiểm tra cuối kỳ',
    examResults: [],
  },
  {
    id: 'ex-3',
    catechismClassId: 'cl-2',
    examDate: '2025-12-20',
    title: 'Kiểm tra giữa kỳ I',
    examResults: [
      { id: 'er-4', examId: 'ex-3', catechismClassEucharisticYouthId: 'cly-6', markedByCatechistId: 'c-2', markedTime: '2025-12-20T10:00:00+07:00', score: 8.0, isDone: true },
    ],
  },
];
