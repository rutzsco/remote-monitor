FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Server/RemoteMonitor.UI.Blazor.Server.csproj", "Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Server/"]
COPY ["Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Client/RemoteMonitor.UI.Blazor.Client.csproj", "Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Client/"]
COPY ["Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Shared/RemoteMonitor.UI.Blazor.Shared.csproj", "Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Shared/"]
COPY ["Services/RemoteMonitor.Services.Ingestion/RemoteMonitor.Services.Ingestion.Shared/RemoteMonitor.Services.Ingestion.Shared.csproj", "Services/RemoteMonitor.Services.Ingestion/RemoteMonitor.Services.Ingestion.Shared/"]
COPY ["Services/RemoteMonitor.Services.Measurements/RemoteMonitor.Services.Measurements.Shared/RemoteMonitor.Services.Measurements.Shared.csproj", "Services/RemoteMonitor.Services.Measurements/RemoteMonitor.Services.Measurements.Shared/"]
COPY ["Services/RemoteMonitor.Services.Topology/RemoteMonitor.Services.Topology.Shared/RemoteMonitor.Services.Topology.Shared.csproj", "Services/RemoteMonitor.Services.Topology/RemoteMonitor.Services.Topology.Shared/"]
RUN dotnet restore "Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Server/RemoteMonitor.UI.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/Services/RemoteMonitor.UI.Blazor/RemoteMonitor.UI.Blazor/Server"

FROM build AS publish
RUN dotnet publish "RemoteMonitor.UI.Blazor.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RemoteMonitor.UI.Blazor.Server.dll"]