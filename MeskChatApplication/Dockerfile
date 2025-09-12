# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY MeskChatApplication.sln .
COPY MeskChatApplication.Application/*.csproj MeskChatApplication.Application/
COPY MeskChatApplication.Domain/*.csproj MeskChatApplication.Domain/
COPY MeskChatApplication.Infrastructure/*.csproj MeskChatApplication.Infrastructure/
COPY MeskChatApplication.Persistance/*.csproj MeskChatApplication.Persistance/
COPY MeskChatApplication.Presentation/*.csproj MeskChatApplication.Presentation/
COPY MeskChatApplication.WebApi/*.csproj MeskChatApplication.WebApi/

# Restore dependencies
RUN dotnet restore MeskChatApplication.sln

# Copy everything else and build
COPY . .
RUN dotnet publish MeskChatApplication.WebApi/MeskChatApplication.WebApi.csproj -c Release -o /app/publish

# 2. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MeskChatApplication.WebApi.dll"]
