# TNTTBD.UI — Frontend Requirements Detail

## Tech Stack
- **Framework**: Angular 21 (standalone components, new control flow)
- **State Management**: Signals + Service (RxJS for HTTP layer only => can HTTP be handled with Signals also?)
- **UI Library**: Angular Material (Material 3)
- **i18n**: Vietnamese + English (with language switcher)
- **App Type**: SPA (standard, no PWA)
- **Responsive**: PC web + mobile web
- **Deployment**: Served by .NET API (same server)

## Layout & Design
- **Navigation**: Top navbar (desktop) + hamburger menu (mobile)
- **Theme**: Light/Dark mode with toggle
- **Colors**: Blue primary + Amber accent (Material palette)
- **Data Display**: Angular Material Table with pagination, sorting, filtering
- **Photos**: Upload from device + camera capture in browser

## Authentication
- SSO: Google + Microsoft (and/or)
- Email/password login
- Public landing page (simple intro + sign-in button) → protected app behind auth

## User Roles & Permissions

| Role | Capabilities |
|------|-------------|
| **Admin** | Full CRUD on all entities; manage users & assign roles; enroll youth/catechists; view everything |
| **Catechist** | CRUD sessions in own classes; mark attendance & exams; view/manage youth in own classes; enroll youth into own class |
| **EucharisticYouthContact** | View/edit their linked youth info; view class basic info (catechists, schedule); submit youth registration |
| **EucharisticYouth** | View own profile + attendance + grades; edit own basic info; view class info (classmates, schedule) |
| **Guest (unauthenticated)** | Access public landing page; submit youth registration form |

## Priority Features (v1)

### 1. Attendance Marking
- Full class student list with toggle buttons (Present / Absent / Excused)
- Catechist selects a session → sees all enrolled youth → marks status
- Mobile-friendly layout for in-class use

### 2. Youth Management
- Profile: FullName (given, family, middle, christ), DOB, address, gender, patron saints, code
- Spiritual Milestones: Baptism, FirstPenance, FirstCommunion, Confirmation, SolemnCommunion, Graduation (date, documentCode, place, note)
- Photos: profile photo + additional photos, upload + camera capture
- Contacts: linked contacts with relationship type

### 3. Class Management
- Catechism Years (start/end date, theme)
- Classes within a year (className)
- Sessions within a class (dateTime, title)
- Enrollment: youth → class (status: Pending, Active, Transferred, Suspended, Completed)
- Catechist assignment: catechist → class (role: Main, Member, Assistance)

### 4. User Management (Admin only)
- CRUD users
- Assign UserRole (Admin, Catechist, EucharisticYouthContact, EucharisticYouth)
- Link user to Catechist / Youth / Contact entity
- Activate/deactivate users

### 5. Dashboard
- Current catechism year info
- Current class(es) assigned to logged-in user
- Quick stats (attendance rate, upcoming sessions, etc.)
- Role-appropriate content

## Guest Youth Registration Flow
- Pure form (no account required to submit)
- After form: ask if inputter has existing account → if yes, link; if no, collect contact info for future account provisioning
- **Required fields**:
  - Youth: FullName (givenName, familyName, middleName, christName), DOB, Date of Baptism
  - Contacts: default Father + Mother (FullName, PhoneNumber, Email, ContactRelationType); can add additional contacts
- No admin approval needed; tracked via enrollment status enum

## Domain Enums Reference
- **UserRole**: Admin, Catechist, EucharisticYouthContact, EucharisticYouth
- **AttendanceStatus**: None, Absent, Excused, Present
- **EnrollmentStatus**: Pending, Active, Transferred, Suspended, Completed
- **MilestoneType**: Baptism, FirstPenance, FirstCommunion, Confirmation, SolemnCommunion, Graduation
- **CatechismClassCatechistRole**: Main, Member, Assistance
- **ContactRelationType**: Undefined, Father, Mother, Guardian, Grandfather, Grandmother, Babysitter, Sibling
- **PhotoPurpose**: Profile, (others)
- **PhotoVisibility**: (TBD from backend)

## Key Value Objects
- **FullName**: givenName (required), christName, familyName, middleName
- **PatronSaint**: saintName, day, month (feast day)