FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SmartTaskManager.API/SmartTaskManager.API.csproj", "SmartTaskManager.API/"]
COPY ["SmartTaskManager.Infrastructure/SmartTaskManager.Infrastructure.csproj", "SmartTaskManager.Infrastructure/"]
COPY ["SmartTaskManager.Core/SmartTaskManager.Core.csproj", "SmartTaskManager.Core/"]

RUN dotnet restore "SmartTaskManager.API/SmartTaskManager.API.csproj"
COPY . .
WORKDIR "/src/SmartTaskManager.API"
RUN dotnet build "SmartTaskManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartTaskManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SmartTaskManager.API.dll"]