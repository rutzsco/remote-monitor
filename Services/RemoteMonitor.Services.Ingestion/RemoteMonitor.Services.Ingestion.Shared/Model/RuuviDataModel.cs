using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Ingestion.Model
{
    public class RuuviPayloadDataModel
    {
        public static RuuviPayloadDataModel CreateFromString(string raw)
        {
            var m = JsonConvert.DeserializeObject<RuuviPayloadDataModel>(raw);
            return m;
        }

        [JsonProperty("data")]
        public RuuviDataModel data { get; set; }
    }

    public class RuuviDataModel
    {
        public static RuuviDataModel CreateFromString(string raw)
        {
            var m = JsonConvert.DeserializeObject<RuuviDataModel>(raw);
            return m;
        }

        [JsonProperty("tags")]
        public Dictionary<string,RuuviTagDataModel> tags { get; set; }
    }

    public class RuuviTagDataModel
    {
        [JsonProperty("data")]
        public string data { get; set; }

        [JsonProperty("timestamp")]
        public long timestamp { get; set; }
    }
}
