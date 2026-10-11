# ==============================================================================
# Multi-Stage Production Dockerfile for .NET 10 Web API
# ==============================================================================

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies as a distinct layer for caching
COPY ["Helpdesk.Api.csproj", "./"]
RUN dotnet restore "Helpdesk.Api.csproj"

# Copy the remaining source code and publish the application
COPY . .
RUN dotnet publish "Helpdesk.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Expose HTTP and HTTPS enterprise ports
EXPOSE 8080
EXPOSE 8081

# Copy published build artifacts from the build stage
COPY --from=build /app/publish .

# Security Best Practice: Run under a non-privileged user account
USER $APP_UID

# Set the application entry point
ENTRYPOINT ["dotnet", "Helpdesk.Api.dll"]