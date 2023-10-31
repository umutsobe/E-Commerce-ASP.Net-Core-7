#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Presentation/e-trade-api.API/e-trade-api.API.csproj", "Presentation/e-trade-api.API/"]
COPY ["Infastructure/e-trade-api.Persistence/e-trade-api.Persistence.csproj", "Infastructure/e-trade-api.Persistence/"]
COPY ["Core/e-trade-api.application/e-trade-api.application.csproj", "Core/e-trade-api.application/"]
COPY ["Core/e-trade-api.domain/e-trade-api.domain.csproj", "Core/e-trade-api.domain/"]
COPY ["Infastructure/e-trade-api.Infastructure/e-trade-api.Infastructure.csproj", "Infastructure/e-trade-api.Infastructure/"]
COPY ["Infastructure/e-trade-api.SignalR/e-trade-api.SignalR.csproj", "Infastructure/e-trade-api.SignalR/"]
RUN dotnet restore "Presentation/e-trade-api.API/e-trade-api.API.csproj"
COPY . .
WORKDIR "/src/Presentation/e-trade-api.API"

RUN dotnet build "e-trade-api.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "e-trade-api.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY cloud.pfx /app/

ENTRYPOINT ["dotnet", "e-trade-api.API.dll"]