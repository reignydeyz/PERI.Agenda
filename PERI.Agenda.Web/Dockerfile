#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base

# Install Node.js
RUN apt-get update -y && apt-get install -y gnupg2 && curl -sL https://deb.nodesource.com/setup_10.x | bash - && apt-get update -y && apt-get install -y nodejs

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PERI.Agenda.Web/PERI.Agenda.Web.csproj", "PERI.Agenda.Web/"]
COPY ["PERI.Agenda.BLL/PERI.Agenda.BLL.csproj", "PERI.Agenda.BLL/"]
COPY ["PERI.Agenda.Repository/PERI.Agenda.Repository.csproj", "PERI.Agenda.Repository/"]
COPY ["PERI.Agenda.EF/PERI.Agenda.EF.csproj", "PERI.Agenda.EF/"]
COPY ["PERI.Agenda.Core/PERI.Agenda.Core.csproj", "PERI.Agenda.Core/"]
RUN dotnet restore "PERI.Agenda.Web/PERI.Agenda.Web.csproj"
COPY . .
WORKDIR "/src/PERI.Agenda.Web"
RUN dotnet build "PERI.Agenda.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PERI.Agenda.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PERI.Agenda.Web.dll"]