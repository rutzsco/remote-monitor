using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Mesurements.Model
{
    public class AggregationStatus
    {
        public AggregationStatus(string deviceId, string aggregation)
        {  
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            Aggregation = aggregation ?? throw new ArgumentNullException(nameof(aggregation));
            Id = $"{deviceId}-{aggregation}";
            LastUpdateTimestamp = DateTime.UtcNow;
            ProcessedTimestamp = null;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        public string DeviceId { get; set; }

        public string Aggregation { get; set; }

        public DateTime LastUpdateTimestamp { get; set; }

        public DateTime? ProcessedTimestamp { get; set; }

    }
}
