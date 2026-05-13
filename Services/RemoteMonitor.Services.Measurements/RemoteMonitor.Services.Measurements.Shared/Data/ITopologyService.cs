using RemoteMonitor.Services.Mesurements.Model;

namespace RemoteMonitor.Services.Measurements.Shared.Data
{
    public interface ITopologyService
    {
        Task<Device> Get(string id);
    }
}