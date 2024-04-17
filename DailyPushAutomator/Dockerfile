FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DailyPushAutomator/DailyPushAutomator.csproj", "DailyPushAutomator/"]
RUN dotnet restore "DailyPushAutomator/DailyPushAutomator.csproj"
COPY . .
WORKDIR "/src/DailyPushAutomator"
RUN dotnet build "DailyPushAutomator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DailyPushAutomator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DailyPushAutomator.dll"]
