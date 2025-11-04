# Banking-System API

A simplified Banking System API built with **.NET Core / ASP.NET Core Web API**.  
Provides core banking operations such as account management, authentication, and transaction handling.

---

## Table of Contents

- [Features](#features)  
- [Architecture & Modules](#architecture--modules)  
- [Getting Started](#getting-started)  
  - [Prerequisites](#prerequisites)  
  - [Setup & Run](#setup--run)  
  - [Database Migration](#database-migration)  
- [Usage & Endpoints](#usage--endpoints)  
- [Configuration](#configuration)  

---

## Features

- User **Authentication & Authorization** (register, login, roles)  
- Account management (create, read, update, delete accounts)  
- Transaction operations (deposit, withdrawal, transfer)  
- Validation, error handling, and API responses  
- Separation of concerns using layers/modules  
- Dependency injection and configuration management  

---

## Architecture & Modules

The solution is organized into these main projects/folders:

- **AuthService** — handles authentication, token issuance, user management  
- **AccountService** — deals with accounts, balances, transactions  
- **Shared** — common utilities, DTOs, models, helpers  
- **Banking-System.sln** — root solution file  

This modular design enables maintainability and separation of concerns.

---

## Getting Started

### Prerequisites

- .NET SDK (.NET 8.0)  
- An SQL Server database   
- (Optional) Postman or similar tool to test the API  

### Setup & Run

1. Clone the repository:

   ```bash
   git clone https://github.com/chaimaJr/Banking-System.git
   cd Banking-System

2. Configure connection strings and secrets in appsettings.json (or appsettings.Development.json) in each project that needs database or authentication settings.

3. Restore packages and build:

  ```bash
  dotnet restore
  dotnet build
  ```
4. Apply migrations / update database:

  ```bash
  cd <service> 
  dotnet ef migrations add <name>
  dotnet ef database update
  ```

5. Run the API:
   ```bash
   dotnet run --project
   dotnet run --project <service>
  

## Usage & Endpoints

Below is a sample list of available API endpoints:

| **HTTP Method** | **Endpoint**                     | **Description**                     | **Auth Required** |
|------------------|----------------------------------|-------------------------------------|-------------------|
| `POST`           | `/api/auth/register`             | Register a new user                 | No                |
| `POST`           | `/api/auth/login`                | Login and receive JWT token         | No                |
| `GET`            | `/api/accounts`                  | Get all accounts                    | Yes               |
| `GET`            | `/api/accounts/{id}`             | Get account details by ID           | Yes               |
| `POST`           | `/api/accounts`                  | Create a new account                | Yes               |
| `PUT`            | `/api/accounts/{id}`             | Update an account                   | Yes               |
| `DELETE`         | `/api/accounts/{id}`             | Delete an account                   | Yes               |
| `POST`           | `/api/accounts/{id}/deposit`     | Deposit funds into an account       | Yes               |
| `POST`           | `/api/accounts/{id}/withdraw`    | Withdraw funds from an account      | Yes               |
| `POST`           | `/api/accounts/transfer`         | Transfer money between accounts     | Yes               |
| `GET`            | `/api/transactions`              | Get all transactions                | Yes               |
| `GET`            | `/api/transactions/{id}`         | Get transaction details by ID       | Yes               |

> **Note:**  
> - Endpoints marked as **Auth Required** must include a valid JWT token in the `Authorization` header:  
>   ```
>   Authorization: Bearer <your_token>
>   ```

## Configuration

All application settings are stored in the `appsettings.json` file (and optionally `appsettings.Development.json` for local use).
Typical settings you’ll find in appsettings.json:

- ConnectionStrings — for your database
- JwtSettings — secret key, token expiration, issuer, audience
- Logging — log levels, console or file
- 
### Example

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BankingDb;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Secret": "YOUR_SECRET_KEY_HERE",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "ExpiryMinutes": 60
  }
  ...

}
  
