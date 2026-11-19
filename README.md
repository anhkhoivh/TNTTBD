# TNTTBD – Eucharistic Youth Management System

A web-based management system for organizing and managing Catechism activities, including member records, catechism classes, attendance, examinations, and related information.

**TNTTBD is an independently developed personal project**, built to explore and apply Domain-Driven Design (DDD), CQRS, and Clean Architecture principles in a practical business application.

The project includes a .NET backend, an Angular frontend, PostgreSQL persistence, and a Docker-based local development environment.

## Project Overview

The application aims to simplify administrative work for Eucharistic Youth organizations by bringing their core management activities into one system.

Key areas include:

- Managing Eucharistic Youth members and catechists.
- Organizing catechism years, classes, and sessions.
- Managing class enrollments and catechist assignments.
- Recording session attendance.
- Managing examinations and student results.
- Managing contacts and relationships with youth members.
- Handling user authentication and role-based access.
- Managing member and catechist photos.

The application is actively developed, with additional features and improvements being introduced over time.

## Technical Stack

| Area | Technologies |
|---|---|
| Backend | C#, ASP.NET Core, .NET |
| Architecture | Domain-Driven Design, CQRS, Clean Architecture |
| Application Layer | Commands, Queries, Handlers (plain DI, no mediator) |
| Data Access | Entity Framework Core |
| Database | PostgreSQL |
| Frontend | Angular, TypeScript |
| Web Server / Reverse Proxy | Nginx |
| Containerization | Docker, Docker Compose |

## Architecture & Design

The backend follows a layered architecture inspired by Domain-Driven Design and Clean Architecture.

### Backend Layers

- **API Layer:** Exposes RESTful endpoints through ASP.NET Core controllers.
- **Application Layer:** Contains use cases, Commands, Queries, Handlers, DTOs, and application abstractions.
- **Domain Layer:** Encapsulates domain entities, aggregates, value objects, and business rules.
- **Infrastructure Layer:** Implements persistence, repositories, QueryServices, and Entity Framework Core integrations.

### CQRS Implementation

The project applies Command Query Responsibility Segregation (CQRS) to separate write operations from read operations.

**Commands – Write Operations**

Commands handle operations that modify application state, such as creating, updating, deleting, and marking attendance.

- Implemented as dedicated command objects.
- Processed by `IRequestHandler` implementations injected directly into controllers, each wrapped in a transactional decorator.
- Use domain entities and aggregates to enforce business rules.
- Persist changes through repositories and the application's persistence mechanisms.

**Queries – Read Operations**

Queries handle data retrieval without modifying application state.

- Processed through dedicated query handlers.
- Use QueryServices for read-oriented database access.
- Project database results into DTOs.
- Support list and paginated data retrieval.

This separation helps keep business operations explicit and makes the application easier to maintain and extend.

## Implemented Functional Areas

The following areas have been implemented in the current project:

### 1. Youth & Catechist Management

- Create, update, and remove youth and catechist records.
- Retrieve individual records and lists.
- Manage patron saints and milestones.
- Manage member-related photos.

### 2. Catechism Management

- Manage catechism years and classes.
- Manage catechism sessions.
- Assign catechists to classes.
- Enroll youth members in classes.
- Track enrollment and completion status.

### 3. Attendance Management

- Record attendance for catechism sessions.
- Support individual and bulk attendance operations.
- Retrieve existing attendance records.

### 4. Examination Management

- Create, update, and remove examinations.
- Associate examinations with catechism classes.
- Record examination results.
- Retrieve examination details and results.

### 5. Contacts & Registration

- Manage contact information.
- Associate contacts with youth members.
- Support youth registration with related contact information.

### 6. Authentication & Authorization

- JWT-based authentication.
- Access token and refresh token handling.
- Refresh token rotation and revocation.
- Google and Microsoft SSO integration.
- Role-based authorization for protected resources.

### 7. Photo Management

- Upload and manage member and catechist photos.
- Organize photos by purpose, such as profile, gallery, and documents.
- Apply authorization rules to photo management operations.
- Store uploaded files using persistent Docker storage.

## Running Locally with Docker

The repository provides a Docker Compose setup for running the application locally.

### Prerequisites

- Docker Desktop with Linux containers enabled.
- Docker Compose.

### Start the application

Clone the repository:

```bash
git clone https://github.com/anhkhoivh/TNTTBD.git
cd TNTTBD
```

Build and start the services:

```bash
docker compose up --build -d
```

Check running containers:

```bash
docker compose ps
```

Check the API health endpoint:

```bash
curl.exe http://localhost:4200/health
```

### Local Service URLs

| Service | URL |
|---|---|
| Angular UI | http://localhost:4200 |
| API through UI proxy | http://localhost:4200/api |
| Direct API access | http://localhost:5000/api |
| pgAdmin | http://localhost:5050 |

The Angular application uses Nginx as a reverse proxy to forward API requests to the backend service.

PostgreSQL data and uploaded photos are persisted through Docker volumes.

**Important:** The credentials configured for the local Docker environment are intended for development only. Replace development credentials and configure appropriate secrets before deploying to any shared or public environment.

## Engineering Highlights

Some of the key engineering aspects explored in this project include:

- Applying DDD concepts to model business entities and aggregates.
- Separating write and read use cases with CQRS.
- Injecting command and query handlers directly through dependency injection, with transactions applied by a decorator.
- Implementing repository and QueryService patterns.
- Using Entity Framework Core for relational persistence.
- Integrating Angular with ASP.NET Core REST APIs.
- Implementing JWT authentication and refresh token handling.
- Handling file uploads and persistent storage.
- Running multiple application services with Docker Compose.
- Troubleshooting and resolving frontend/backend integration issues.

## Project Status

**Status: Actively Developed**

This is an ongoing personal project. Core management workflows and the main backend/frontend integration are implemented, while additional features, refinements, and improvements continue to evolve.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Repository

[https://github.com/anhkhoivh/TNTTBD](https://github.com/anhkhoivh/TNTTBD)