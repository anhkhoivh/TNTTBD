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
  { id: 'u-admin', email: 'admin@tnttbd.org', userName: 'admin', isActive: true, userRole: UserRole.Admin },
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
    address: 'Giáo xứ Bình Đông',
    photos: [photo('cat-1')],
    patronSaints: [{ saintName: 'Phêrô', day: 29, month: 6 }],
  },
  {
    id: 'c-2',
    fullName: { givenName: 'Mai', familyName: 'Trần', christName: 'Maria' },
    dateOfBirth: '1997-08-05',
    isMale: false,
    address: 'Giáo xứ Bình Đông',
    photos: [photo('cat-2')],
    patronSaints: [{ saintName: 'Maria', day: 15, month: 8 }],
  },
  {
    id: 'c-3',
    fullName: { givenName: 'Hoà', familyName: 'Vũ', christName: 'Gioan' },
    dateOfBirth: '1999-11-20',
    isMale: true,
    address: 'Giáo xứ Bình Đông',
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
  address: 'Giáo xứ Bình Đông',
  photos: [photo(id)],
  patronSaints: [{ saintName: christ, day: 1, month: 1 }],
  milestones: milestones.map((m, i) => ({
    id: `m-${id}-${i}`,
    type: m.type,
    date: m.date,
    place: m.place ?? 'Giáo xứ Bình Đông',
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
  { id: 'yc-1', eucharisticYouthId: 'y-1', contactId: 'co-1', contactRelationType: ContactRelationType.Father,
    contact: { id: 'co-1', fullName: { givenName: 'Hùng', familyName: 'Nguyễn' }, phoneNumber: '0901234567', email: 'parent1@tnttbd.local' } },
  { id: 'yc-2', eucharisticYouthId: 'y-2', contactId: 'co-2', contactRelationType: ContactRelationType.Mother,
    contact: { id: 'co-2', fullName: { givenName: 'Lan', familyName: 'Lê' }, phoneNumber: '0907654321', email: 'parent2@tnttbd.local' } },
  { id: 'yc-3', eucharisticYouthId: 'y-3', contactId: 'co-3', contactRelationType: ContactRelationType.Mother,
    contact: { id: 'co-3', fullName: { givenName: 'Tâm', familyName: 'Phạm' }, phoneNumber: '0912345678' } },
  { id: 'yc-4', eucharisticYouthId: 'y-4', contactId: 'co-4', contactRelationType: ContactRelationType.Father,
    contact: { id: 'co-4', fullName: { givenName: 'Hồng', familyName: 'Hoàng' }, phoneNumber: '0987654321' } },
  { id: 'yc-5', eucharisticYouthId: 'y-1', contactId: 'co-2', contactRelationType: ContactRelationType.Mother,
    contact: { id: 'co-2', fullName: { givenName: 'Lan', familyName: 'Lê' }, phoneNumber: '0907654321', email: 'parent2@tnttbd.local' } },
];

export const mockYears: CatechismYear[] = [
  { id: 'yr-2024', startDate: '2024-09-01', endDate: '2025-06-30', theme: 'Yêu thương và phục vụ', title: '2024-2025' },
  { id: 'yr-2025', startDate: '2025-09-01', endDate: '2026-06-30', theme: 'Sống Lời Chúa', title: '2025-2026' },
];

export const mockClasses: CatechismClass[] = [
  { id: 'cl-1', catechismYearId: 'yr-2025', className: 'Khai tâm 1', classCode: 'KT1', catechismYearTitle: "2025-2026" },
  { id: 'cl-2', catechismYearId: 'yr-2025', className: 'Rước lễ 1', classCode: 'KT1', catechismYearTitle: "2025-2026" },
  { id: 'cl-3', catechismYearId: 'yr-2025', className: 'Thêm sức 2', classCode: 'KT1', catechismYearTitle: "2025-2026" },
  { id: 'cl-4', catechismYearId: 'yr-2025', className: 'Bao đồng', classCode: 'KT1', catechismYearTitle: "2025-2026" },
  { id: 'cl-prev-1', catechismYearId: 'yr-2024', className: 'Khai tâm 1 (2024)', classCode: 'KT1', catechismYearTitle: "2025-2026" },
];

export const mockClassCatechists: CatechismClassCatechist[] = [
  // yr-2025: each catechist in exactly ONE class
  { id: 'clc-1', catechismClassId: 'cl-1', catechistId: 'c-1', role: CatechismClassCatechistRole.Main, enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'clc-2', catechismClassId: 'cl-1', catechistId: 'c-2', role: CatechismClassCatechistRole.Member, enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  { id: 'clc-4', catechismClassId: 'cl-2', catechistId: 'c-3', role: CatechismClassCatechistRole.Main, enrolledAt: '2025-09-01T08:00:00+07:00', status: EnrollmentStatus.Active },
  // yr-2024 history (completed)
  { id: 'clc-h1', catechismClassId: 'cl-prev-1', catechistId: 'c-1', role: CatechismClassCatechistRole.Main, enrolledAt: '2024-09-01T08:00:00+07:00', leftAt: '2025-06-30T17:00:00+07:00', status: EnrollmentStatus.Completed },
  { id: 'clc-h2', catechismClassId: 'cl-prev-1', catechistId: 'c-3', role: CatechismClassCatechistRole.Member, enrolledAt: '2024-09-01T08:00:00+07:00', leftAt: '2025-06-30T17:00:00+07:00', status: EnrollmentStatus.Completed },
];

export const mockClassYouth: CatechismClassEucharisticYouth[] = [

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
  { id: 'a-1', catechismClassEucharisticYouthId: 'cly-1', attendanceStatus: AttendanceStatus.Present },
  { id: 'a-2', catechismClassEucharisticYouthId: 'cly-2', attendanceStatus: AttendanceStatus.Absent },
  { id: 'a-3', catechismClassEucharisticYouthId: 'cly-3', attendanceStatus: AttendanceStatus.Present },
  { id: 'a-4', catechismClassEucharisticYouthId: 'cly-4', attendanceStatus: AttendanceStatus.Excused },
  { id: 'a-5', catechismClassEucharisticYouthId: 'cly-1', attendanceStatus: AttendanceStatus.Present },
  { id: 'a-6', catechismClassEucharisticYouthId: 'cly-2', attendanceStatus: AttendanceStatus.Present },
];

export const mockExams: Exam[] = [
  {
    id: 'ex-1',
    catechismClassId: 'cl-1',
    examDate: '2025-12-15',
    title: 'Kiểm tra giữa kỳ I',
    examResults: [
      { id: 'er-1', catechismClassEucharisticYouthId: 'cly-1', score: 8.5 },
      { id: 'er-2', catechismClassEucharisticYouthId: 'cly-2', score: 7.0},
      { id: 'er-3', catechismClassEucharisticYouthId: 'cly-3', score: 9.0 },
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
      { id: 'er-4', catechismClassEucharisticYouthId: 'cly-6', score: 8.0 },
    ],
  },
];
