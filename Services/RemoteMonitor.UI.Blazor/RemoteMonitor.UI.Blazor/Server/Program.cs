using Microsoft.Azure.Cosmos;
using RemoteMonitor.Services.Mesurements.Data;
using RemoteMonitor.Services.Topology.Data;
using RemoteMonitor.UI.Blazor.Server;
using RemoteMonitor.UI.Blazor.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetValue<string>("CosmosDBConnectionString")
        ?? configuration.GetValue<string>("CosmosDBConnection")
        ?? throw new InvalidOperationException("CosmosDBConnectionString is required.");

    return new CosmosClient(connectionString);
});

var azureSignalRConnectionString = builder.Configuration.GetValue<string>("AzureSignalRConnectionString");
if (string.IsNullOrWhiteSpace(azureSignalRConnectionString))
{
    var signalRUrl = builder.Configuration.GetValue<string>("AzureSignalRUrl");
    var signalRKey = builder.Configuration.GetValue<string>("AzureSignalRKey");
    if (!string.IsNullOrWhiteSpace(signalRUrl) && !string.IsNullOrWhiteSpace(signalRKey))
    {
        azureSignalRConnectionString = $"Endpoint={signalRUrl};AccessKey={signalRKey};Version=1.0;";
    }
}

var signalRBuilder = builder.Services.AddSignalR();
if (!string.IsNullOrWhiteSpace(azureSignalRConnectionString))
{
    signalRBuilder.AddAzureSignalR(azureSignalRConnectionString);
}

builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped(sp => new MeasurementService(
    sp.GetRequiredService<ILoggerFactory>().CreateLogger("MeasurementService"),
    sp.GetRequiredService<CosmosClient>(),
    sp.GetRequiredService<RemoteMonitor.Services.Measurements.Shared.Data.ITopologyService>()));

builder.Services.AddScoped<ITopologyService, LocalTopologyService>();
builder.Services.AddScoped<IMeasurementService, LocalMeasurementService>();
builder.Services.AddScoped<RemoteMonitor.Services.Measurements.Shared.Data.ITopologyService, MeasurementTopologyService>();
builder.Services.AddScoped<RemoteMonitor.Services.Ingestion.Shared.Services.IMeasurementService, LocalIngestionMeasurementService>();
builder.Services.AddScoped<MeasurementPipeline>();
builder.Services.AddScoped<ObservationService>();
builder.Services.AddScoped<TelemetryIngestionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<MeasurementsHub>("/measurements");
app.MapFallbackToFile("index.html");

app.Run();
