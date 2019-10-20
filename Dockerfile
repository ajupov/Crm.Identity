FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY Crm.Identity/. ./Crm.Identity/
WORKDIR /app/Crm.Identity
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/Crm.Identity/out ./
ENTRYPOINT ["dotnet", "Crm.Identity.dll"]
EXPOSE 9000