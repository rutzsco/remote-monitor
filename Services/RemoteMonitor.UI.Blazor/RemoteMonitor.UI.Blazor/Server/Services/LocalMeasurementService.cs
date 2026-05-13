using RemoteMonitor.Services.Mesurements.Data;
using RemoteMonitor.UI.Models;
using MeasurementSummary = RemoteMonitor.Services.Mesurements.Model.MeasurementSummary;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public class LocalMeasurementService : IMeasurementService
    {
        private readonly MeasurementService _measurementService;

        public LocalMeasurementService(MeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        public async Task<DeviceMeasurementsViewModel> Get(string id)
        {
            var measurements = await GetRaw(id);
            var uiMeasurements = measurements.Select(MeasurementMapper.ToUiMeasurement);
            return DeviceMeasurementsViewModel.CreateFromMeasurements(uiMeasurements);
        }

        public Task<IEnumerable<MeasurementSummary>> GetRaw(string id)
        {
            return _measurementService.GetHistoricalMesurementsAsync(id);
        }
    }
}