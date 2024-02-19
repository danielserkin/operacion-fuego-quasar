# Usa la imagen de .NET 7 SDK como base
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY . .

RUN dotnet publish /app/src/OperacionFuegoQasar.Api -c Release -o /app/src/OperacionFuegoQasar.Api/out

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/src/OperacionFuegoQasar.Api/out ./

ENTRYPOINT ["dotnet", "OperacionFuegoQasar.Api.dll"]
