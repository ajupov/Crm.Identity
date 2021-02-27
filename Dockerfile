FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /app

ARG NUGET_SOURCE
ARG NUGET_USERNAME
ARG NUGET_PASSWORD

RUN echo "<?xml version=\"1.0\" encoding=\"utf-8\"?>" > NuGet.config
RUN echo "<configuration>" >> NuGet.config
RUN echo "<packageSources>" >> NuGet.config
RUN echo "<add key=\"nuget.org\" value=\"https://api.nuget.org/v3/index.json\" protocolVersion=\"3\" />" >> NuGet.config
RUN echo "<add key=\"GPR\" value=\"$NUGET_SOURCE\" />" >> NuGet.config
RUN echo "</packageSources>" >> NuGet.config
RUN echo "<packageSourceCredentials>" >> NuGet.config
RUN echo "<GPR>" >> NuGet.config
RUN echo "<add key=\"Username\" value=\"$NUGET_USERNAME\" />" >> NuGet.config
RUN echo "<add key=\"ClearTextPassword\" value=\"$NUGET_PASSWORD\" />" >> NuGet.config
RUN echo "</GPR>" >> NuGet.config
RUN echo "</packageSourceCredentials>" >> NuGet.config
RUN echo "</configuration>" >> NuGet.config

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Crm.Identity.dll"]
EXPOSE 3000 3001
