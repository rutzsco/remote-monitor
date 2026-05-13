using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RemoteMonitor.UI.Blazor.Shared
{
    public class Observation
    {
        public Observation(string id, string deviceId, string type, string value, DateTime timestamp)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Count = 1;
            Timestamp = timestamp;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        public int Count { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
