using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Mesurements.Model
{
    public class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public Guid SiteId { get; set; }

        public string DeviceModelId { get; set; }

        public int TemperatureOffset { get; set; }

        public int HumidityOffset { get; set; }
    }
}
