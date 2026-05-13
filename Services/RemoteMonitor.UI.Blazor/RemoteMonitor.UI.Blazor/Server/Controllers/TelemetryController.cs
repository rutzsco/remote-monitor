using Microsoft.AspNetCore.Mvc;
using RemoteMonitor.UI.Blazor.Server.Services;

namespace RemoteMonitor.UI.Blazor.Server.Controllers
{
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        private readonly TelemetryIngestionService _telemetryIngestionService;
        private readonly ILogger<TelemetryController> _logger;

        public TelemetryController(TelemetryIngestionService telemetryIngestionService, ILogger<TelemetryController> logger)
        {
            _telemetryIngestionService = telemetryIngestionService;
            _logger = logger;
        }

        [HttpPost("telemetry-events")]
        public async Task<ActionResult> EventHubTelemetry([FromBody] System.Text.Json.JsonElement telemetryEvent)
        {
            try
            {
                await _telemetryIngestionService.ProcessCloudEventAsync(telemetryEvent);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing telemetry event.");
                return StatusCode(500);
            }
        }

        [HttpPost("api/RuuviMeasurementEndpoint")]
        public async Task<ActionResult> RuuviMeasurementEndpoint()
        {
            using var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            await _telemetryIngestionService.ProcessRuuviPayloadAsync(requestBody);
            return new OkObjectResult("OK");
        }

        [HttpGet("status")]
        public IActionResult Get()
        {
            return new OkObjectResult("OK");
        }
    }
}