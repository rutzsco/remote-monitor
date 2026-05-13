using Microsoft.AspNetCore.SignalR;
using RemoteMonitor.Services.Mesurements.Data;
using RemoteMonitor.Services.Mesurements.Model;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public class MeasurementPipeline
    {
        private readonly MeasurementService _measurementService;
        private readonly ObservationService _observationService;
        private readonly IHubContext<MeasurementsHub> _hubContext;

        public MeasurementPipeline(MeasurementService measurementService, ObservationService observationService, IHubContext<MeasurementsHub> hubContext)
        {
            _measurementService = measurementService;
            _observationService = observationService;
            _hubContext = hubContext;
        }

        public async Task ProcessMeasurementsAsync(IEnumerable<MeasurementSummary> measurements)
        {
            var summaries = await _measurementService.CreateMesurementsAsync(measurements.ToArray());
            foreach (var summary in summaries)
            {
                await _hubContext.Clients.All.SendAsync("measurement", MeasurementMapper.ToNotification(summary));
                await _observationService.TrackMovementAsync(summary);
            }
        }
    }
}