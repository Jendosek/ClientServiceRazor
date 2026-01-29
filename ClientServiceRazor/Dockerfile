FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClientServiceRazor/ClientServiceRazor.csproj", "ClientServiceRazor/"]
RUN dotnet restore "ClientServiceRazor/ClientServiceRazor.csproj"
COPY . .
WORKDIR "/src/ClientServiceRazor"
RUN dotnet build "./ClientServiceRazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClientServiceRazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientServiceRazor.dll"]
