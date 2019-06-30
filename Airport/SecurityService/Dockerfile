FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SecurityService/SecurityService.csproj", "SecurityService/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "SecurityService/SecurityService.csproj"
COPY . .
WORKDIR "/src/SecurityService"
RUN dotnet build "SecurityService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SecurityService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Airport.SecurityService.dll"]