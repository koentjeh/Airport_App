FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CustomerManagementAPI/CustomerManagementAPI.csproj", "CustomerManagementAPI/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "CustomerManagementAPI/CustomerManagementAPI.csproj"
COPY . .
WORKDIR /src/CustomerManagementAPI
RUN dotnet build "CustomerManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CustomerManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Airport.CustomerManagementAPI.dll"]