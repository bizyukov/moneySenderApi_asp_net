# Parrot Wings – ASP.NET Core 2.2 Backend API

[![.NET Core](https://img.shields.io/badge/.NET%20Core-2.2-blue.svg)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-2.2-purple.svg)](https://docs.microsoft.com/ef/)
[![JWT](https://img.shields.io/badge/JWT-Auth-orange.svg)](https://jwt.io/)
[![Status: Archived](https://img.shields.io/badge/status-archived-lightgrey.svg)](https://github.com/yourusername/parrot-wings-api)

> **⚠️ EDUCATIONAL PROJECT – DO NOT USE IN PRODUCTION**  
> This repository contains a **simple REST API** built with **ASP.NET Core 2.2** as a learning exercise. It has **outdated dependencies** and **security flaws**. It is preserved **for portfolio and educational reference only**.

## 📖 Overview

This backend powers a basic payment/wallet system (often called "Parrot Wings"). It provides endpoints for:

- User registration and JWT‑based authentication.
- Retrieving user information.
- Listing other users.
- Creating money transactions between users.

The frontend counterpart is an **Angular 8** application (see [Parrot Wings Angular Client](https://github.com/yourusername/parrot-wings-angular)).

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 2.2
- **Database**: SQL Server (via Entity Framework Core)
- **Authentication**: JWT (Bearer tokens)
- **Serialization**: Newtonsoft.Json

## 📁 Project Structure
```bash
dotnetWebApi/
├── Controllers/
│ ├── SessionsController.cs # Login / JWT issuance
│ ├── UsersController.cs # User registration and profile
│ ├── UsersListController.cs # User search
│ └── TransactionsController.cs # Transaction creation and history
├── Models/
│ ├── User.cs
│ ├── Transaction.cs
│ └── TransactionExt.cs
├── Migrations/ # EF Core migrations
├── AppDataBaseContext.cs # EF Core DbContext
├── AuthOptions.cs # JWT configuration
├── Startup.cs
└── Program.cs
```

## ✅ Implemented Features

- User registration with email/password.
- JWT token generation on login.
- Protected endpoints using `[Authorize]` attribute.
- Basic transaction logic (sender balance decreases, receiver increases).
- Transaction history with enriched user names.

## 🚫 Critical Issues & Security Warnings

| Issue | Description |
|-------|-------------|
| **MD5 Password Hashing** | Passwords are hashed with MD5 – **cryptographically broken**. |
| **Hardcoded JWT Secret** | `AuthOptions.KEY` is hardcoded in source code. |
| **No Input Validation** | Minimal validation; lacks DTOs and proper model validation. |
| **Outdated Framework** | .NET Core 2.2 is **end‑of‑life** (no security updates). |
| **Mixed Database Concerns** | Controllers directly use `AppDataBaseContext`; no service layer. |
| **No Tests** | Zero unit or integration tests. |

**Do not deploy this API publicly.** It is provided for educational purposes only.

## 📚 Educational Value

This project is a practical example of:

- Setting up **JWT authentication** in ASP.NET Core.
- Using **Entity Framework Core** with code‑first migrations.
- Building a **REST API** with basic CRUD operations.
- Handling **cross‑origin requests** (CORS).

It complements the **Angular frontend** to form a complete full‑stack application, demonstrating end‑to‑end development skills.

## 🔧 How to Run (for Educational Purposes)

```bash
# Update the connection string in Startup.cs
dotnet ef database update
dotnet run
```
The API will be available at https://localhost:44330 (or similar). You will need to run the Angular frontend separately.

📄 License
MIT – use freely for learning.

👤 Author & EB‑1A Context
GitHub: @bizyukov
This repository is part of a documented portfolio showcasing 15+ years of software development, supporting an EB‑1A extraordinary ability visa petition under the original contributions criterion.

“The only way to learn a new programming language is by writing programs in it.” – Dennis Ritchie