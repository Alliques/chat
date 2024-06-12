#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tools/DbMigrator/DbMigrator.csproj", "Tools/DbMigrator/"]
RUN dotnet restore "Tools/DbMigrator/DbMigrator.csproj"
COPY . .
WORKDIR "/src/Tools/DbMigrator"
RUN dotnet build "DbMigrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DbMigrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DbMigrator.dll"]