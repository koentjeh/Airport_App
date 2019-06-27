FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CustomerManagementAPI/CustomerManagementAPI.csproj", "CustomerManagementAPI/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "CustomerManagementAPI/CustomerManagementAPI.csproj"

COPY . .
WORKDIR "/src/CustomerManagementAPI"
RUN dotnet build "CustomerManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CustomerManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Expose ports
EXPOSE 5100/tcp
ENV ASPNETCORE_URLS http://*:5100
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5100/hc || exit 1

#start
ENTRYPOINT ["dotnet", "Airport.CustomerManagementAPI.dll"]