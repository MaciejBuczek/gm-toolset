FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER app
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

LABEL org.opencontainers.image.source=https://github.com/maciejbuczek/gm-toolset


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY src/Services/Identity/Identity.API/Identity.API.csproj Services/Identity/Identity.API/

RUN dotnet restore "Services/Identity/Identity.API/Identity.API.csproj"

COPY src/ .

WORKDIR "/src/Services/Identity/Identity.API"

RUN dotnet build "Identity.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build


FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Identity.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Identity.API.dll"]