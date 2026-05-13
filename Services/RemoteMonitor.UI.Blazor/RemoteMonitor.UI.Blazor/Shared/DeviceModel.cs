using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RemoteMonitor.UI.Models
{
    public class DeviceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Guid SiteId { get; set; }

        public string DeviceModelId { get; set; }
    }

    public class Device
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("siteId")]
        public Guid SiteId { get; set; }

        [JsonPropertyName("deviceModelId")]
        public string DeviceModelId { get; set; }

        [JsonPropertyName("temperatureOffset")]
        public int TemperatureOffset { get; set; }

        [JsonPropertyName("humidityOffset")]
        public int HumidityOffset { get; set; }
    }
}
