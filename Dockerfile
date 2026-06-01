FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SmartAquapark.Api/SmartAquapark.Api.csproj SmartAquapark.Api/
COPY SmartAquapark.Application/SmartAquapark.Application.csproj SmartAquapark.Application/
COPY SmartAquapark.Domain/SmartAquapark.Domain.csproj SmartAquapark.Domain/
COPY SmartAquapark.Infrastructure/SmartAquapark.Infrastructure.csproj SmartAquapark.Infrastructure/

RUN dotnet restore SmartAquapark.Api/SmartAquapark.Api.csproj

COPY . .

RUN dotnet publish SmartAquapark.Api/SmartAquapark.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SmartAquapark.Api.dll"]