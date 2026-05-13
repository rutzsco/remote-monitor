using Microsoft.AspNetCore.Mvc;
using RemoteMonitor.UI.Blazor.Server.Services;
using RemoteMonitor.UI.Models;
using MeasurementSummary = RemoteMonitor.Services.Mesurements.Model.MeasurementSummary;

using System.Text.Json;

namespace RemoteMonitor.UI.Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementsController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly MeasurementPipeline _measurementPipeline;

        public MeasurementsController(IMeasurementService measurementService, MeasurementPipeline measurementPipeline)
        {
            _measurementService = measurementService;
            _measurementPipeline = measurementPipeline;
        }

        // GET: api/Measurements/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<DeviceMeasurementsViewModel> Get(string id)
        {
            var vm = await _measurementService.Get(id);
            return vm;
        }

        [HttpPost]
        [Route("/Measurements")]
        public async Task<ActionResult> AddMesurementsEndpoint(IEnumerable<MeasurementSummary> measurements)
        {
            await _measurementPipeline.ProcessMeasurementsAsync(measurements);
            return new OkObjectResult("OK");
        }

        [HttpGet("/Measurements/{deviceId}")]
        public async Task<ActionResult> GetMesurementsEndpoint(string deviceId)
        {
            var historicalListing = await _measurementService.GetRaw(deviceId);
            return new OkObjectResult(historicalListing);
        }

        [HttpGet("Status")]
        [Route("/Measurements/Status")]
        public ActionResult Status()
        {
            return new OkObjectResult("OK");
        }
    }
}