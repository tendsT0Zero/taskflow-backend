# 🚀 TaskFlow - Enterprise Agile Board (Backend API)

TaskFlow is a robust, scalable, and secure robust RESTful Web API built with **.NET 8** following **Clean Architecture** principles. It serves as the backend for an enterprise-level Kanban board application (similar to Jira or Trello), designed to manage workspaces, projects, and task flows efficiently.

## 🏗️ Architecture

The project strictly follows **Clean Architecture** to ensure separation of concerns, testability, and maintainability:

* **App.Domain:** Contains Enterprise-wide logic and Types (Entities, Enums, Base Classes).
* **App.Application:** Contains Business logic, Interfaces, and DTOs (Data Transfer Objects).
* **App.Infrastructure:** Contains Data Access logic (Entity Framework Core, DbContext, Identity configuration, Repositories/Services).
* **App.Api:** The presentation layer containing Controllers, Dependency Injection setup, and Swagger configurations.

## 🛠️ Tech Stack

* **Framework:** .NET 8.0 (ASP.NET Core Web API)
* **Language:** C# 12
* **Database:** SQL Server
* **ORM:** Entity Framework Core (EF Core)
* **Authentication & Security:** ASP.NET Core Identity & JWT (JSON Web Tokens)
* **Documentation:** Swagger / OpenAPI

## ✨ Features Implemented (So Far)

* **Standardized API Responses:** Implemented a global generic `ApiResponse<T>` wrapper for consistent frontend consumption.
* **Authentication Module:** * Secure User Registration & Login using Identity Core.
    * JWT Generation with claims extraction.
* **Workspace Module:**
    * Create isolated workspaces per user.
    * Retrieve all workspaces owned by the authenticated user.
* **Project & Board Module:**
    * Create new projects under specific workspaces.
    * **Auto-initialization:** Automatically creates default Kanban columns ("To Do", "In Progress", "Done") upon project creation.
    * Eager loading of columns structured by their order.

## 🚦 Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server (LocalDB or Express)
* Visual Studio 2022, Rider, or VS Code

### Installation & Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/tendsT0Zero/TaskFlow.git](https://github.com/tendsT0Zero/TaskFlow.git)
    cd TaskFlow
    ```

2.  **Configure the Database & JWT Settings:**
    Open `App.Api/appsettings.json` (or `appsettings.Development.json`) and update the `ConnectionStrings` and `JwtSettings`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskFlowDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    },
    "JwtSettings": {
      "Secret": "YOUR_SUPER_SECRET_LONG_KEY_HERE!",
      "Issuer": "TaskFlowBackend",
      "Audience": "TaskFlowFrontend",
      "ExpiryMinutes": 120
    }
    ```

3.  **Run Entity Framework Migrations:**
    Open your terminal in the root folder and run:
    ```bash
    dotnet ef database update --project App.Infrastructure --startup-project App.Api
    ```

4.  **Run the API:**
    ```bash
    cd App.Api
    dotnet run
    ```

5.  **Explore via Swagger:**
    Navigate to `https://localhost:<port>/swagger` in your browser. 
    *(Note: To test secured endpoints, first use `/api/auth/login` to get a token, then click the **Authorize** button in Swagger and enter `Bearer <your-token>`).*

## 🛣️ Roadmap / Upcoming Features

- [ ] Task & Sub-task CRUD operations.
- [ ] Drag-and-drop column movement logic (Reordering).
- [ ] User role management (Admin/Member) and Invites.
- [ ] Task Comments & Activity Audit Logs.

---
*Developed with ❤️ as an API-First Backend Architecture.*