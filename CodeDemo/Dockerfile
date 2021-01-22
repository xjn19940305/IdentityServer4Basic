#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CodeDemo/CodeDemo.csproj", "CodeDemo/"]
RUN dotnet restore "CodeDemo/CodeDemo.csproj"
COPY . .
WORKDIR "/src/CodeDemo"
RUN dotnet build "CodeDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeDemo.dll"]