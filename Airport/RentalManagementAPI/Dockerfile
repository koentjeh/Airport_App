FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["RentalManagementAPI/RentalManagementAPI.csproj", "RentalManagementAPI/"]
RUN dotnet restore "RentalManagementAPI/RentalManagementAPI.csproj"
COPY . .
WORKDIR "/src/RentalManagementAPI"
RUN dotnet build "RentalManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RentalManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RentalManagementAPI.dll"]