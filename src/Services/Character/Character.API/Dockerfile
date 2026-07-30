FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER app
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

LABEL org.opencontainers.image.source=https://github.com/maciejbuczek/gm-toolset


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY src/Services/Character/Character.API/Character.API.csproj Services/Character/Character.API/

RUN dotnet restore "Services/Character/Character.API/Character.API.csproj"

COPY src/ .

WORKDIR "/src/Services/Character/Character.API"

RUN dotnet build "Character.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build


FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Character.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Character.API.dll"]