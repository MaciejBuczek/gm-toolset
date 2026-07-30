FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER app
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

LABEL org.opencontainers.image.source=https://github.com/maciejbuczek/gm-toolset


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy only project file first for better docker cache
COPY src/Gateway/Gateway.API/Gateway.API.csproj Gateway/Gateway.API/

RUN dotnet restore "Gateway/Gateway.API/Gateway.API.csproj"

# Copy source
COPY src/ .

WORKDIR "/src/Gateway/Gateway.API"

RUN dotnet build "Gateway.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build


FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Gateway.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Gateway.API.dll"]