FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["InvoiceService/InvoiceService.csproj", "InvoiceService/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "InvoiceService/InvoiceService.csproj"
COPY . .
WORKDIR "/src/InvoiceService"
RUN dotnet build "InvoiceService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "InvoiceService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Airport.InvoiceService.dll"]