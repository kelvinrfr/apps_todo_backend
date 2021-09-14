# TodoApp.Data
Project responsible for database conection and database migrations

# Migrations
While in `/src/backend/TodoApp.Data` execute:
- Adding a new migration: `dotnet ef migrations add [MIGRATION_NAME]`
- Updating database to with latest changes: `dotnet ef database update`