using System.Text.Json;
using RemoteMonitor.Services.Ingestion.Logic;
using RemoteMonitor.Services.Ingestion.Shared.Logic;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public class TelemetryIngestionService
    {
        private readonly MeasurementPipeline _measurementPipeline;
        private readonly ILogger<TelemetryIngestionService> _logger;

        public TelemetryIngestionService(MeasurementPipeline measurementPipeline, ILogger<TelemetryIngestionService> logger)
        {
            _measurementPipeline = measurementPipeline;
            _logger = logger;
        }

        public async Task ProcessCloudEventAsync(JsonElement telemetryEvent)
        {
            var data = telemetryEvent.TryGetProperty("data", out var dataElement)
                ? dataElement.GetRawText()
                : telemetryEvent.GetRawText();

            var measurements = RuuviTelemetryProcessor.ProcessCloudEvent(data, _logger);
            await ProcessIngestionMeasurementsAsync(measurements);
        }

        public async Task ProcessRuuviPayloadAsync(string requestBody)
        {
            var measurements = RuuviTelemetryProcessor.Process(requestBody, _logger);
            await ProcessIngestionMeasurementsAsync(measurements);
        }

        private async Task ProcessIngestionMeasurementsAsync(IEnumerable<RemoteMonitor.Services.Ingestion.Model.MeasurementSummary> measurements)
        {
            var deduped = MeasurementSummaryDeduplication.Execute(measurements);
            await _measurementPipeline.ProcessMeasurementsAsync(deduped.Select(MeasurementMapper.ToMeasurement));
        }
    }
}