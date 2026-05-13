using Microsoft.AspNetCore.Mvc;
using RemoteMonitor.UI.Blazor.Server.Services;
using RemoteMonitor.UI.Models;

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace RemoteMonitor.UI.Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ITopologyService _topologyService;
        private ILogger _logger;

        public DeviceController(ITopologyService topologyService, ILoggerFactory logger)
        {
            _topologyService = topologyService;
            _logger = logger.CreateLogger("DeviceController");
        }

        // GET: api/device/{id}
        [HttpGet("{id}")]
        public async Task<Device> Get(string id)
        {
            var result = await _topologyService.Get(id);
            return result;
        }

        // GET: api/device
        [HttpGet]
        public async Task<IEnumerable<DeviceSummary>> Get()
        {
            _logger.LogInformation("Calling topology service for device listing.");
            var result = await _topologyService.GetList();
            return result;
        }

        [HttpPost]
        public async Task Post([FromBody] AddDeviceCommand command)
        {
            await _topologyService.Add(command);
        }

        [HttpPost("Configuration")]
        public async Task Post([FromBody] UpdateDeviceConfigurationCommand command)
        {
            await _topologyService.Update(command);
        }


        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _topologyService.Delete(id);
        }
    }
}
