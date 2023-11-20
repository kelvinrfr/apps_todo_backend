FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/TodoApp.Api.Http/TodoApp.Api.Http.csproj ./src/TodoApp/
COPY src/TodoApp.Infrastructure/TodoApp.Infrastructure.csproj ./src/TodoApp.Infrastructure/
COPY src/TodoApp.Application/TodoApp.Application.csproj ./src/TodoApp.Application/
COPY src/TodoApp.Domain/TodoApp.Domain.csproj ./src/TodoApp.Domain/
COPY TodoApp.sln ./
RUN dotnet restore TodoApp.sln

COPY . ./
RUN dotnet publish -c Release -o out src/TodoApp.Api.Http/TodoApp.Api.Http.csproj

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TodoApp.Api.Http.dll"]