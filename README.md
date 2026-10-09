# Exam projects

This repository contains three ASP.NET Core MVC projects:

- `ExamMVC`
- `Test`
- `examfinal`

## Database configuration

Local `appsettings*.json` files are ignored because they can contain database credentials. For a fresh checkout, copy the matching `appsettings.example.json` to `appsettings.json` and replace the placeholder password with your local PostgreSQL password.

`examfinal` includes `Database.sql` for creating its tables.

## Run a project

From this folder, run one project at a time:

```powershell
dotnet run --project .\ExamMVC\ExamMVC.csproj
dotnet run --project .\Test\Test.csproj
dotnet run --project .\examfinal\examfinal.csproj
```
