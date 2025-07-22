> [!WARNING]
> This project is prototype and designed for demonstration purposes. It cannot be used in production.
 
# MatrixBugtracker

A simple bug-tracker project created for demonstration in [Matrix Academy](https://matrixacademy.edu.az). Inspired by [VK's bug-tracker](https://vk.com/testing). 

This is a platform where testers help developers and managers in the company find and fix problems in their products.

The company's __employees__ create and manage __products__ in the platform. __User__ as testers create __reports__ in the products. __Moderators__ checks the reports for the fact of reproducing the bug specified in the reports and change its status.

It is possible to leave __comments__ on reports and attach __files__ to reports and comments. Testers can mark other reports as "reproduced".

There is a notifications system. Sending notifications by e-mail to users has been implemented.

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* An Entity Framework Core compatible database (SQL Server, MySQL, etc.). Please note that this project uses SQL Server by default.
* `dotnet-ef` tool:
```
dotnet tool install --global dotnet-ef --version 8.0.17
```

## Getting started

### 1. Clone this repository

```
git clone https://github.com/Elorucov/MatrixBugtracker.git
cd MatrixBugtracker
```

### Configure the project

Don't forget change these values in `appsettings.json`: 

* `ConnectionStrings > DefaultConnection` (connection string to point to your database)
* `Serilog > WriteTo[1] > Args > path` (path to the folder where the logs will be stored)
* `PathForUploadedFiles` (path to the folder where the uploaded files will be stored)
* `EmailConfig` (сonfiguration of the email address on behalf of which notifications will be sent)

### Run migrations

```
dotnet ef database update --project MatrixBugtracker.API
```

### Run the project

```
dotnet run --project MatrixBugtracker.API
```

The API should now be running locally at `http://localhost:5196`.

> __Note:__ For detailed API documentation, refer to the Swagger documentation available at `http://localhost:5196/swagger` when the application is running.

## Technologies and principles used

* Serilog
* AutoMapper
* FluentValidator
* _JWT_ and _refresh_ tokens
* Unit-tests with xUnit and Moq
* N-Layer architecture
* Design following SOLID Principles
* Repository and Unit-of-Work pattern
