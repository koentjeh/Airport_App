FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CustomerManagementAPI/CustomerManagementAPI.csproj", "CustomerManagementAPI/"]
RUN dotnet restore "CustomerManagementAPI/CustomerManagementAPI.csproj"
COPY . .
WORKDIR "/src/CustomerManagementAPI"
RUN dotnet build "CustomerManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CustomerManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CustomerManagementAPI.dll"]