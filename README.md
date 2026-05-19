# RemoteMonitor

RemoteMonitor is a Blazor WebAssembly application with an ASP.NET Core server backend for viewing and ingesting device telemetry. The server is built as a container image by GitHub Actions and can be deployed directly to Azure Container Apps.

## Container Image

The GitHub Actions workflow in `.github/workflows/docker-image.yml` publishes the UI/server container to GitHub Container Registry:

```text
ghcr.io/<github-owner>/<github-repo>/remote-monitor
```

For this repository, use:

```text
ghcr.io/rutzsco/remote-monitor/remote-monitor:latest
```

The workflow also publishes immutable SHA tags. Prefer a SHA tag for production deployments and `latest` for quick test environments.

## Azure Resources

The container app needs these Azure resources:

- Azure Container Apps environment.
- Azure Cosmos DB account. The app creates the `Measurements` database and required containers if they do not already exist.
- Azure SignalR Service. This is optional for local testing, but recommended in Azure because the app hosts a SignalR hub at `/measurements`.
- Application Insights or Log Analytics for telemetry and diagnostics.

## Deploy To Azure Container Apps

Prerequisites:

- Azure CLI with the Container Apps extension installed: `az extension add --name containerapp --upgrade`.
- An active Azure login: `az login`.
- Access to the GitHub Container Registry package. Public packages need no registry credentials; private packages require a GitHub personal access token with `read:packages`.

Set deployment variables first:

```powershell
$resourceGroup = "rg-remote-monitor"
$location = "eastus"
$environment = "cae-remote-monitor"
$containerApp = "remote-monitor"
$image = "ghcr.io/rutzsco/remote-monitor/remote-monitor:latest"
```

Create the resource group and Container Apps environment:

```powershell
az group create `
	--name $resourceGroup `
	--location $location

az containerapp env create `
	--name $environment `
	--resource-group $resourceGroup `
	--location $location
```

Create the container app from the GitHub-hosted image:

```powershell
az containerapp create `
	--name $containerApp `
	--resource-group $resourceGroup `
	--environment $environment `
	--image $image `
	--target-port 8080 `
	--ingress external `
	--min-replicas 1
```

If the GitHub Container Registry package is private, include registry credentials when creating the app:

```powershell
$githubUser = "<github-user>"
$githubPat = "<github-pat-with-read-packages>"

az containerapp create `
	--name $containerApp `
	--resource-group $resourceGroup `
	--environment $environment `
	--image $image `
	--target-port 8080 `
	--ingress external `
	--min-replicas 1 `
	--registry-server ghcr.io `
	--registry-username $githubUser `
	--registry-password $githubPat
```

For an existing app that needs registry credentials added or rotated, run:

```powershell
$githubUser = "<github-user>"
$githubPat = "<github-pat-with-read-packages>"

az containerapp registry set `
	--name $containerApp `
	--resource-group $resourceGroup `
	--server ghcr.io `
	--username $githubUser `
	--password $githubPat
```

## Configure The Container App

Configure application settings as Container Apps secrets, then expose them as environment variables. The app reads ASP.NET Core configuration from environment variables.

Required setting:

| Setting | Description |
| --- | --- |
| `CosmosDBConnectionString` | Cosmos DB connection string. `CosmosDBConnection` is also supported as a fallback. |

Recommended settings:

| Setting | Description |
| --- | --- |
| `AzureSignalRConnectionString` | Azure SignalR Service connection string. |
| `APPLICATIONINSIGHTS_CONNECTION_STRING` | Application Insights connection string for telemetry. |
| `ASPNETCORE_ENVIRONMENT` | Use `Production` for Azure deployments. |

Instead of `AzureSignalRConnectionString`, the app can also build the connection string from these two settings:

| Setting | Description |
| --- | --- |
| `AzureSignalRUrl` | Azure SignalR endpoint, such as `https://<name>.service.signalr.net`. |
| `AzureSignalRKey` | Azure SignalR access key. |

Apply the configuration:

```powershell
$cosmosConnectionString = "<cosmos-db-connection-string>"
$signalRConnectionString = "<azure-signalr-connection-string>"
$applicationInsightsConnectionString = "<application-insights-connection-string>"

az containerapp secret set `
	--name $containerApp `
	--resource-group $resourceGroup `
	--secrets `
		cosmos-db-connection-string="$cosmosConnectionString" `
		azure-signalr-connection-string="$signalRConnectionString" `
		applicationinsights-connection-string="$applicationInsightsConnectionString"

az containerapp update `
	--name $containerApp `
	--resource-group $resourceGroup `
	--set-env-vars `
		ASPNETCORE_ENVIRONMENT=Production `
		CosmosDBConnectionString=secretref:cosmos-db-connection-string `
		AzureSignalRConnectionString=secretref:azure-signalr-connection-string `
		APPLICATIONINSIGHTS_CONNECTION_STRING=secretref:applicationinsights-connection-string
```

Get the public URL:

```powershell
az containerapp show `
	--name $containerApp `
	--resource-group $resourceGroup `
	--query properties.configuration.ingress.fqdn `
	--output tsv
```

## Update An Existing Deployment

When GitHub Actions publishes a new image, update the Container App image:

```powershell
az containerapp update `
	--name $containerApp `
	--resource-group $resourceGroup `
	--image $image
```

Use an immutable SHA tag for controlled rollouts:

```powershell
$image = "ghcr.io/rutzsco/remote-monitor/remote-monitor:sha-<commit-sha>"
```

