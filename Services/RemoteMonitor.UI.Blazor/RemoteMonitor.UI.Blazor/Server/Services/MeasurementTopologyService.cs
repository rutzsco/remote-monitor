using RemoteMonitor.Services.Topology.Data;
using MeasurementTopologyServiceInterface = RemoteMonitor.Services.Measurements.Shared.Data.ITopologyService;
using MeasurementDevice = RemoteMonitor.Services.Mesurements.Model.Device;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public class MeasurementTopologyService : MeasurementTopologyServiceInterface
    {
        private readonly DeviceService _deviceService;

        public MeasurementTopologyService(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public async Task<MeasurementDevice> Get(string id)
        {
            try
            {
                var device = await _deviceService.GetDeviceByIdAsync(id);
                return new MeasurementDevice
                {
                    Id = device.Id,
                    Name = device.Name,
                    TemperatureOffset = device.TemperatureOffset,
                    HumidityOffset = device.HumidityOffset
                };
            }
            catch (Microsoft.Azure.Cosmos.CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new MeasurementDevice { Id = id, HumidityOffset = 0, TemperatureOffset = 0 };
            }
        }
    }
}