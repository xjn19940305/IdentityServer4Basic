#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node:14 as nodejs
WORKDIR /src
COPY . .
WORKDIR /src/IdsEFCore/ClientApp
RUN npm install -g cnpm --registry=https://registry.npm.taobao.org
RUN cnpm install 
RUN cnpm run build


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["IdsEFCore/IdsEFCore.csproj", "IdsEFCore/"]
COPY ["Dependency/Dependency.csproj", "Dependency/"]
COPY ["IDS.Database/IDS.Database.csproj", "IDS.Database/"]
COPY ["IDS.MySql/IDS.MySql.csproj", "IDS.MySql/"]
RUN dotnet restore "IdsEFCore/IdsEFCore.csproj"
COPY . .
WORKDIR "/src/IdsEFCore"
RUN dotnet build "IdsEFCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdsEFCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=nodejs /src/IdsEFCore/ClientApp/dist/ ./wwwroot/
RUN rm /app/appsetting*
ENTRYPOINT ["dotnet", "IdsEFCore.dll"]