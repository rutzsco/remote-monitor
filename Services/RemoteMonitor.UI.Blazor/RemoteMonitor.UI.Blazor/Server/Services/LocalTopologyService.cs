using RemoteMonitor.Services.Topology.Data;
using RemoteMonitor.UI.Models;
using TopologyDevice = RemoteMonitor.Services.Topology.Model.Device;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public class LocalTopologyService : ITopologyService
    {
        private readonly DeviceService _deviceService;

        public LocalTopologyService(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public async Task Add(AddDeviceCommand command)
        {
            var device = new TopologyDevice
            {
                Id = command.Id,
                Name = command.Name,
                HumidityOffset = 0,
                TemperatureOffset = 0
            };

            await _deviceService.AddUpdateDevice(device);
        }

        public async Task Delete(string id)
        {
            await _deviceService.DeleteDevice(id);
        }

        public async Task<Device> Get(string id)
        {
            try
            {
                var device = await _deviceService.GetDeviceByIdAsync(id);
                return MeasurementMapper.ToUiDevice(device);
            }
            catch (Microsoft.Azure.Cosmos.CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new Device { Id = id, HumidityOffset = 0, TemperatureOffset = 0 };
            }
        }

        public async Task<IEnumerable<DeviceSummary>> GetList()
        {
            var devices = await _deviceService.GetDevicesBySiteAsync();
            return devices.Select(MeasurementMapper.ToUiDeviceSummary);
        }

        public async Task Update(UpdateDeviceConfigurationCommand command)
        {
            var device = new TopologyDevice
            {
                Id = command.Id,
                Name = command.Name,
                HumidityOffset = int.Parse(command.HumidityOffset),
                TemperatureOffset = int.Parse(command.TemperatureOffset)
            };

            await _deviceService.AddUpdateDevice(device);
        }
    }
}