Library Management System
Console-based library manager built with .NET 9 and C# 13. Follows Clean Architecture — clear separation of UI, business logic, and data access. Easy to maintain, extend, and test.

Features
Add, update, delete, and search books
Fetch by ID or ISBN
Input validation & custom exceptions
Clean, menu-driven console UI
NUnit test suite for core logic

How It’s Structured
UI — Console interface (menu, input/output)
Service — Business rules, validation, error handling
Infrastructure — In-memory repo (swap for DB later)
Common — DTOs, interfaces, exceptions
Tests — Unit tests (NUnit)

Getting Started
Prerequisites:
.NET 9 SDK

Build:

bash
Copy
Edit
dotnet build
Run:

bash
Copy
Edit
dotnet run --project LibraryManagementSystem/LibraryManagementSystem.UI.csproj
Run Tests:

bash
Copy
Edit
dotnet test LibraryManagementSystem.Tests/LibraryManagementSystem.Tests.csproj
Extending
Plug in a real DB: implement IBookRepository and IUnitOfWork with EF Core or similar.

Add new features: expand the service and UI layers.

Solution Layout
arduino
Copy
Edit
LibraryManagementSystem/             Console UI
LibraryManagementSystem.Service/     Business logic
LibraryManagementSystem.Infrastructure.DAL/   Data access
LibraryManagementSystem.Common/      DTOs, interfaces, exceptions
LibraryManagementSystem.Tests/       NUnit tests
