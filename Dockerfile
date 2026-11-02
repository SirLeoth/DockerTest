
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Projektdatei kopieren
COPY DockerTest/DockerTest/DockerTest.csproj DockerTest/DockerTest/

# NuGet-Pakete wiederherstellen
RUN dotnet restore DockerTest/DockerTest/DockerTest.csproj

# Restlichen Projektcode kopieren
COPY DockerTest/DockerTest/ DockerTest/DockerTest/

# Anwendung veröffentlichen
RUN dotnet publish DockerTest/DockerTest/DockerTest.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

# Port für Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "DockerTest.dll"]
