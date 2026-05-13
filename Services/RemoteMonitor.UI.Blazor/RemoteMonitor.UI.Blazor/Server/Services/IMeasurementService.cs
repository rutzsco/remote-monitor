using RemoteMonitor.UI.Models;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public interface IMeasurementService
    {
        Task<DeviceMeasurementsViewModel> Get(string id);
        Task<IEnumerable<RemoteMonitor.Services.Mesurements.Model.MeasurementSummary>> GetRaw(string id);
    }
}
