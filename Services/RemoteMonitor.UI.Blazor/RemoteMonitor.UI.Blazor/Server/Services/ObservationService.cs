using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using RemoteMonitor.Services.Mesurements.Model;
using RemoteMonitor.UI.Blazor.Shared;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public class ObservationService
    {
        private const string DatabaseName = "Measurements";
        private const string ObservationContainerName = "Observation";
        private const string TrackerContainerName = "DeviceTracker";
        private readonly CosmosClient _cosmosClient;

        public ObservationService(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<IEnumerable<Observation>> GetByDeviceIdAsync(string deviceId)
        {
            var container = await GetContainerAsync(ObservationContainerName);
            var query = new QueryDefinition("SELECT * FROM o WHERE o.deviceId = @deviceId AND o.timestamp >= @timestamp")
                .WithParameter("@deviceId", deviceId)
                .WithParameter("@timestamp", DateTime.UtcNow.AddDays(-3));

            var observations = new List<Observation>();
            using var iterator = container.GetItemQueryIterator<ObservationDocument>(query);
            while (iterator.HasMoreResults)
            {
                foreach (var observation in await iterator.ReadNextAsync())
                {
                    observations.Add(new Observation(observation.Id, observation.DeviceId, observation.Type, observation.Value, observation.Timestamp)
                    {
                        Count = observation.Count
                    });
                }
            }

            return observations;
        }

        public async Task TrackMovementAsync(MeasurementSummary measurement)
        {
            if (!string.Equals(measurement.MeasurementType, MeasurementSummary.MeasurementTypeSummary, StringComparison.OrdinalIgnoreCase) || !measurement.MovementCount.HasValue)
            {
                return;
            }

            var trackerContainer = await GetContainerAsync(TrackerContainerName);
            var tracker = await GetTrackerAsync(trackerContainer, measurement.DeviceId);
            if (tracker.MovementCount == measurement.MovementCount.Value)
            {
                return;
            }

            tracker.MovementCount = measurement.MovementCount.Value;
            await trackerContainer.UpsertItemAsync(tracker, new PartitionKey(tracker.DeviceId));

            var observation = new ObservationDocument
            {
                Id = Guid.NewGuid().ToString(),
                DeviceId = measurement.DeviceId,
                Type = "MovementEvent",
                Value = measurement.MovementCount.Value.ToString(),
                Count = 1,
                Timestamp = measurement.Timestamp
            };

            var observationContainer = await GetContainerAsync(ObservationContainerName);
            await observationContainer.UpsertItemAsync(observation, new PartitionKey(observation.DeviceId));
        }

        private async Task<Container> GetContainerAsync(string containerName)
        {
            var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName);
            var container = await database.Database.CreateContainerIfNotExistsAsync(containerName, "/deviceId");
            return container.Container;
        }

        private static async Task<DeviceTrackerDocument> GetTrackerAsync(Container container, string deviceId)
        {
            try
            {
                return await container.ReadItemAsync<DeviceTrackerDocument>(deviceId, new PartitionKey(deviceId));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new DeviceTrackerDocument { Id = deviceId, DeviceId = deviceId };
            }
        }

        private class DeviceTrackerDocument
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;

            [JsonProperty("deviceId")]
            public string DeviceId { get; set; } = string.Empty;

            [JsonProperty("movementCount")]
            public int MovementCount { get; set; }
        }

        private class ObservationDocument
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;

            [JsonProperty("deviceId")]
            public string DeviceId { get; set; } = string.Empty;

            [JsonProperty("type")]
            public string Type { get; set; } = string.Empty;

            [JsonProperty("value")]
            public string Value { get; set; } = string.Empty;

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("timestamp")]
            public DateTime Timestamp { get; set; }
        }
    }
}