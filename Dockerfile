FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /app

COPY src/*.csproj ./
COPY NuGet.config ./
RUN dotnet restore

COPY src ./
COPY src/appsettings.json ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime
WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Crm.Identity.dll"]
EXPOSE 3000 3001