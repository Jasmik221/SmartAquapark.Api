FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SmartAquapark.Api/SmartAquapark.Api.csproj SmartAquapark.Api/
COPY SmartAquapark.Application/SmartAquapark.Application.csproj SmartAquapark.Application/
COPY SmartAquapark.Domain/SmartAquapark.Domain.csproj SmartAquapark.Domain/
COPY SmartAquapark.Infrastructure/SmartAquapark.Infrastructure.csproj SmartAquapark.Infrastructure/

RUN dotnet restore SmartAquapark.Api/SmartAquapark.Api.csproj

COPY . .

RUN dotnet publish SmartAquapark.Api/SmartAquapark.Api.csproj -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:11.0 AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

LABEL org.opencontainers.image.source="https://github.com/Jasmik221/SmartAquapark.Api"
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
USER $APP_UID
EXPOSE 8080

HEALTHCHECK --interval=10s --timeout=5s --start-period=30s --retries=6 \
    CMD curl --fail --silent http://localhost:8080/health/ready || exit 1

ENTRYPOINT ["dotnet", "SmartAquapark.Api.dll"]
