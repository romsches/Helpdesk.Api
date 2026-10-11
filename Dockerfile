# Этап сборки (Build Stage)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Helpdesk.Api.csproj", "./"]
RUN dotnet restore "Helpdesk.Api.csproj"

COPY . .
RUN dotnet publish "Helpdesk.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап выполнения (Runtime Stage)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Helpdesk.Api.dll"]