FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["RentalService/RentalService.csproj", "RentalService/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "RentalService/RentalService.csproj"
COPY . .
WORKDIR "/src/RentalService"
RUN dotnet build "RentalService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RentalService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Airport.RentalService.dll"]