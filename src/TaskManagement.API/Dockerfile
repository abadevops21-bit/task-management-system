FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY src/TaskManagement.API/TaskManagement.API.csproj src/TaskManagement.API/
COPY src/TaskManagement.Application/TaskManagement.Application.csproj src/TaskManagement.Application/
COPY src/TaskManagement.Domain/TaskManagement.Domain.csproj src/TaskManagement.Domain/
COPY src/TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj src/TaskManagement.Infrastructure/

# Restore
RUN dotnet restore src/TaskManagement.API/TaskManagement.API.csproj

# Copy full source
COPY . .

# Build
WORKDIR /src/src/TaskManagement.API
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "TaskManagement.API.dll"]