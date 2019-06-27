FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["FlightManagementAPI/FlightManagementAPI.csproj", "FlightManagementAPI/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "FlightManagementAPI/FlightManagementAPI.csproj"
COPY . .
WORKDIR /src/FlightManagementAPI
RUN dotnet build "FlightManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FlightManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Expose ports
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5000/hc || exit 1

ENTRYPOINT ["dotnet", "Airport.FlightManagementAPI.dll"]