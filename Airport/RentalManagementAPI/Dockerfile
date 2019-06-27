FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["RentalManagementAPI/RentalManagementAPI.csproj", "RentalManagementAPI/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "RentalManagementAPI/RentalManagementAPI.csproj"
COPY . .
WORKDIR /src/RentalManagementAPI
RUN dotnet build "RentalManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RentalManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Expose ports
EXPOSE 5200/tcp
ENV ASPNETCORE_URLS http://*:5200
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5200/hc || exit 1

ENTRYPOINT ["dotnet", "Airport.RentalManagementAPI.dll"]