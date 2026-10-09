# 🎫 Mini Helpdesk / Ticket-System Web API

A professional, production-ready RESTful Web API built with **.NET 10** and **PostgreSQL**. Designed with clean architecture principles, robust error handling, and enterprise-grade validation to demonstrate high-level backend engineering skills.

---

## 🚀 Tech Stack

- **Framework:** .NET 10 (ASP.NET Core Web API)
- **Database:** PostgreSQL (Containerized via Docker)
- **ORM:** Entity Framework (EF) Core (Code-First Approach)
- **Validation:** FluentValidation (Separation of concerns)
- **Logging:** Serilog (Structured console logging with performance metrics)
- **Documentation:** Swagger / OpenAPI UI

---

## 🏛️ Architecture & Design Patterns

The application follows standard enterprise patterns to ensure high maintainability, testability, and resilience:

1. **Global Exception Handling Middleware:** Centralized error management that intercepts unhandled exceptions, logs them with full stack context via Serilog, and returns standardized JSON error responses.
2. **FluentValidation Pipeline:** Isolated validation logic keeping domain models clean and business rules strictly enforced.
3. **Robust Data Fallbacks:** Automated injection of default safety values (`Anonymous`, `Unassigned`) for strict database `NOT NULL` constraints, preventing unhandled database exceptions on incomplete client payloads.

---

## 📊 Database Schema (`Ticket`)

| Column | Type | Constraints / Notes |
| :--- | :--- | :--- |
| `Id` | Integer | Primary Key, Auto-increment |
| `Title` | String | Max 100 chars, Required |
| `Description` | String | Required |
| `Status` | String | Default: `"Open"` (`Open`, `InProgress`, `Closed`) |
| `Priority` | String | Default: `"Medium"` (`Low`, `Medium`, `High`) |
| `CreatedBy` | String | Fallback injected if empty |
| `AssignedTo` | String | Fallback injected if empty |
| `CreatedAt` | DateTime | UTC Timestamp |
| `ResolvedAt` | DateTime? | Nullable resolution timestamp |

---

## 🔌 API Endpoints (RESTful)

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/api/tickets` | Retrieve all tickets (Supports status filtering via `?status=Open`) |
| **GET** | `/api/tickets/{id}` | Retrieve a specific ticket by ID |
| **POST** | `/api/tickets` | Create a new ticket with automatic validation and fallbacks |
| **PUT** | `/api/tickets/{id}` | Update an existing ticket |
| **DELETE**| `/api/tickets/{id}` | Remove a ticket from the system |

---

## ⚙️ Getting Started & Installation

### Prerequisites
- .NET 10 SDK installed
- Docker (for running PostgreSQL)

### 1. Clone the repository
```bash
git clone [https://github.com/romsches/Helpdesk.Api.git](https://github.com/romsches/Helpdesk.Api.git)
cd Helpdesk.Api