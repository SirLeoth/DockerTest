# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Projektdatei kopieren
COPY *.csproj ./
RUN dotnet restore

# Restlichen Code kopieren
COPY . ./

# Anwendung veröffentlichen
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Render verwendet den Port aus der Umgebungsvariable PORT
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "DockerTest.dll"]
