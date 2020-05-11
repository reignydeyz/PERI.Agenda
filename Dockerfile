FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY PERI.Agenda.Web/*.csproj PERI.Agenda.Web/
RUN dotnet restore "PERI.Agenda.Web/PERI.Agenda.Web.csproj"

# Copy everything else and build
COPY . ./
WORKDIR "PERI.Agenda.Web"
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "PERI.Agenda.Web.dll"]