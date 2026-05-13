
using Microsoft.Azure.Cosmos;
using RemoteMonitor.Services.Measurements.Shared.Data;
using RemoteMonitor.Services.Mesurements.Model;
using Microsoft.Extensions.Logging;

namespace RemoteMonitor.Services.Mesurements.Data
{
    public class MeasurementService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ITopologyService _topologyService;
        private readonly ILogger _logger;
        public MeasurementService(ILogger logger, CosmosClient cosmosClient, ITopologyService topologyService)
        {
            _cosmosClient = cosmosClient;
            _topologyService = topologyService;
            _logger = logger;
        }

        public async Task<IEnumerable<MeasurementSummary>> CreateMesurementsAsync(IEnumerable<MeasurementSummary> measurements)
        {
            var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Measurements");
            var container = await db.Database.CreateContainerIfNotExistsAsync("Records", "/DeviceId");
            var summaries = new List<MeasurementSummary>();

            foreach (var mesurement in measurements.OrderBy(x => x.DeviceId))
            {
                try
                {
                    var device = await _topologyService.Get(mesurement.DeviceId);
                    mesurement.ApplyCalibration(device.TemperatureOffset, device.HumidityOffset);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unable to process calibration: {ex.Message}", ex);
                }

                var m1 = await container.Container.UpsertItemAsync(item: mesurement, partitionKey: new PartitionKey(mesurement.DeviceId));

                var m2 = new MeasurementSummary(mesurement.DeviceId, MeasurementSummary.MeasurementTypeSummary, mesurement.Temperature, mesurement.Humidity, mesurement.Pressure, mesurement.MovementCount, mesurement.Timestamp);
                var m2r = await container.Container.UpsertItemAsync(item: m2, partitionKey: new PartitionKey(m2.DeviceId));
                summaries.Add(m2);

                var m3 = new MeasurementSummary(mesurement.DeviceId, MeasurementSummary.MeasurementTypeAggregation, mesurement.Temperature, mesurement.Humidity, mesurement.Pressure, mesurement.MovementCount, mesurement.Timestamp);
                var m3r = await container.Container.UpsertItemAsync(item: m3, partitionKey: new PartitionKey(m3.DeviceId));
            }

            return summaries;
        }

        public async Task<IEnumerable<MeasurementSummary>> GetHistoricalMesurementsAsync(string deviceId)
        {
            var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Measurements");
            var container = await db.Database.CreateContainerIfNotExistsAsync("Records", "/DeviceId");

            QueryDefinition query = new QueryDefinition("SELECT * FROM r WHERE r.DeviceId = @deviceId AND r.MeasurementType = @measurementType AND r.Timestamp > @timestamp")
                .WithParameter("@deviceId", deviceId)
                .WithParameter("@measurementType", MeasurementSummary.MeasurementTypeAggregation)
                .WithParameter("@timestamp", DateTime.UtcNow.AddDays(-3));

            List<MeasurementSummary> results = new();
            using (FeedIterator<MeasurementSummary> resultSetIterator = container.Container.GetItemQueryIterator<MeasurementSummary>(query, requestOptions: new QueryRequestOptions(){ }))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<MeasurementSummary> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response);
                }
            }

            var groups = results.OrderByDescending(x => x.Timestamp).GroupBy(x =>
            {
                var stamp = x.Timestamp;
                stamp = stamp.AddMinutes(-(stamp.Minute % 30));
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            }).Select(g => g.First()).ToList();

            return groups;
        }
    }
}