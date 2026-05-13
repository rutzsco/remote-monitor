using IngestionMeasurementSummary = RemoteMonitor.Services.Ingestion.Model.MeasurementSummary;
using IngestionMeasurementService = RemoteMonitor.Services.Ingestion.Shared.Services.IMeasurementService;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public class LocalIngestionMeasurementService : IngestionMeasurementService
    {
        private readonly MeasurementPipeline _measurementPipeline;

        public LocalIngestionMeasurementService(MeasurementPipeline measurementPipeline)
        {
            _measurementPipeline = measurementPipeline;
        }

        public async Task AddMeasurements(IEnumerable<IngestionMeasurementSummary> measurements)
        {
            await _measurementPipeline.ProcessMeasurementsAsync(measurements.Select(MeasurementMapper.ToMeasurement));
        }
    }
}