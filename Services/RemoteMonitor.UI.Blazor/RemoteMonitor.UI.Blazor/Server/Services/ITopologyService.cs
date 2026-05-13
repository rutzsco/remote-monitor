using RemoteMonitor.UI.Models;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public interface ITopologyService
    {
        Task Add(AddDeviceCommand command);
        Task Delete(string id);
        Task<Device> Get(string id);
        Task<IEnumerable<DeviceSummary>> GetList();
        Task Update(UpdateDeviceConfigurationCommand command);
    }
}