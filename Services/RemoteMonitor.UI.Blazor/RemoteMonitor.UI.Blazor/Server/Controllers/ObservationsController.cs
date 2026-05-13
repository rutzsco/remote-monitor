using Microsoft.AspNetCore.Mvc;
using RemoteMonitor.UI.Blazor.Server.Services;

namespace RemoteMonitor.UI.Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/observations")]
    public class ObservationsController : ControllerBase
    {
        private readonly ObservationService _observationService;

        public ObservationsController(ObservationService observationService)
        {
            _observationService = observationService;
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetByDeviceId(string deviceId)
        {
            var observations = await _observationService.GetByDeviceIdAsync(deviceId);
            return new OkObjectResult(observations);
        }
    }
}