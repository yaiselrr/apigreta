FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Greta.BO.Worker/Greta.BO.Worker.csproj", "src/Greta.BO.Worker/"]
COPY ["src/Greta.BO.Api.Sqlserver/Greta.BO.Api.Sqlserver.csproj", "src/Greta.BO.Api.Sqlserver/"]
COPY ["src/Greta.BO.Api.Abstractions/Greta.BO.Api.Abstractions.csproj", "src/Greta.BO.Api.Abstractions/"]
COPY ["src/Greta.BO.Api.Entities/Greta.BO.Api.Entities.csproj", "src/Greta.BO.Api.Entities/"]
COPY ["src/Greta.BO.BusinessLogic/Greta.BO.BusinessLogic.csproj", "src/Greta.BO.BusinessLogic/"]
RUN dotnet restore "src/Greta.BO.Worker/Greta.BO.Worker.csproj"
COPY . .
WORKDIR "/src/src/Greta.BO.Worker"
RUN dotnet build "Greta.BO.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Greta.BO.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Greta.BO.Worker.dll"]
