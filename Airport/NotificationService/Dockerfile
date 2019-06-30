FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["NotificationService/NotificationService.csproj", "NotificationService/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "NotificationService/NotificationService.csproj"
COPY . .
WORKDIR "/src/NotificationService"
RUN dotnet build "NotificationService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NotificationService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Airport.NotficationService.dll"]