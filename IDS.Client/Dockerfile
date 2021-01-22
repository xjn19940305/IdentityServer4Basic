#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["IDS.Client/IDS.Client.csproj", "IDS.Client/"]
COPY ["Dependency/Dependency.csproj", "Dependency/"]
COPY ["IDS.Database/IDS.Database.csproj", "IDS.Database/"]
COPY ["IDS.MySql/IDS.MySql.csproj", "IDS.MySql/"]
RUN dotnet restore "IDS.Client/IDS.Client.csproj"
COPY . .
WORKDIR "/src/IDS.Client"
RUN dotnet build "IDS.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IDS.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDS.Client.dll"]