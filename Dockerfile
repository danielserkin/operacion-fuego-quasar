#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/OperacionFuegoQasar.Api/OperacionFuegoQasar.Api.csproj", "src/OperacionFuegoQasar.Api/"]
COPY ["src/OperacionFuegoQuasar.Application/OperacionFuegoQuasar.Application.csproj", "src/OperacionFuegoQuasar.Application/"]
COPY ["src/OperacionFuegoQuasar.Domain/OperacionFuegoQuasar.Domain.csproj", "src/OperacionFuegoQuasar.Domain/"]
COPY ["src/OperacionFuegoQuasar.Infrastructure/OperacionFuegoQuasar.Infrastructure.csproj", "src/OperacionFuegoQuasar.Infrastructure/"]
COPY ["src/OperacionFuegoQasar.Api/operacion-fuego-qasar.db", "src/OperacionFuegoQasar.Api/"]

RUN dotnet restore "src/OperacionFuegoQasar.Api/OperacionFuegoQasar.Api.csproj"
COPY . .
WORKDIR "/src/src/OperacionFuegoQasar.Api"
RUN dotnet build "OperacionFuegoQasar.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OperacionFuegoQasar.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OperacionFuegoQasar.Api.dll"]