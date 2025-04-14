FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and all project files
COPY GestaoTarefas.sln .

# App projects
COPY src/GestaoTarefas.Api/*.csproj ./src/GestaoTarefas.Api/
COPY src/GestaoTarefas.Application/*.csproj ./src/GestaoTarefas.Application/
COPY src/GestaoTarefas.Domain/*.csproj ./src/GestaoTarefas.Domain/
COPY src/GestaoTarefas.Infrastructure/*.csproj ./src/GestaoTarefas.Infrastructure/

# Test projects
COPY tests/GestaoTarefas.Domain.Tests/*.csproj ./tests/GestaoTarefas.Domain.Tests/
COPY tests/GestaoTarefas.Application.Tests/*.csproj ./tests/GestaoTarefas.Application.Tests/
COPY tests/GestaoTarefas.Infrastructure.Tests/*.csproj ./tests/GestaoTarefas.Infrastructure.Tests/
COPY tests/GestaoTarefas.Api.Tests/*.csproj ./tests/GestaoTarefas.Api.Tests/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY src/. ./src/
COPY tests/. ./tests/

# Build and publish
RUN dotnet publish src/GestaoTarefas.Api/GestaoTarefas.Api.csproj -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "GestaoTarefas.Api.dll"]
