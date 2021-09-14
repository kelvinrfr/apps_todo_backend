FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY TodoApp/TodoApp.csproj ./TodoApp/
COPY TodoApp.Data/TodoApp.Data.csproj ./TodoApp.Data/
COPY TodoApp.sln ./
RUN dotnet restore TodoApp.sln

COPY . ./
RUN dotnet publish -c Release -o out TodoApp/TodoApp.csproj

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TodoApp.dll"]