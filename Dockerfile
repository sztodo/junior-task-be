FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# 1. Copiem tot conținutul (inclusiv .slnx, src și tests)
COPY . .

# 2. Rulăm Publish direct. 
# În .NET 10, publish face automat restore și build. 
# Fiind totul într-o singură comandă, nu mai există riscul să "piardă" pachetele între layere.
RUN dotnet publish "src/Api/Api.csproj" -c Release -o /app/publish

# ── Runtime stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]