# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TombolaGame/*.csproj TombolaGame/
COPY TombolaGame.Tests/*.csproj TombolaGame.Tests/
RUN dotnet restore TombolaGame/TombolaGame.csproj

COPY . .
WORKDIR /src/TombolaGame
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TombolaGame.dll"]