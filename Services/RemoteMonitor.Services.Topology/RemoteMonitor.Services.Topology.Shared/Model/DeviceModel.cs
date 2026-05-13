using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Topology.Model
{
    public class DeviceModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
