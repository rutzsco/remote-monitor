using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RemoteMonitor.Services.Ingestion.Model
{
    public class MeasurementSummary : IEquatable<MeasurementSummary>
    {
        public MeasurementSummary(string deviceId, string measurementType, decimal temperature, decimal humidity, decimal pressure, int? movementCount, DateTime timestamp)
        {
            DeviceId = deviceId;
            MeasurementType = measurementType;
            Humidity = humidity;
            Temperature = temperature;
            Pressure = pressure;
            MovementCount = movementCount;

            if (measurementType.ToLower() == "summary")
            {
                Id = deviceId;
            }
            else
                Id = Guid.NewGuid().ToString();

            Timestamp = timestamp;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        public string DeviceId { get; set; }

        public string MeasurementType { get; set; }

        public decimal Humidity { get; set; }

        public decimal Temperature { get; set; }

        public decimal Pressure { get; set; }

        [JsonProperty("movementCount")]
        public int? MovementCount { get; set; }


        public DateTime Timestamp { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as MeasurementSummary);
        }

        public bool Equals([AllowNull] MeasurementSummary other)
        {
            return other != null &&
                   DeviceId == other.DeviceId &&
                   MeasurementType == other.MeasurementType &&
                   Humidity == other.Humidity &&
                   Temperature == other.Temperature &&
                   Pressure == other.Pressure &&
                   MovementCount == other.MovementCount &&
                   Timestamp == other.Timestamp;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(DeviceId);
            hash.Add(MeasurementType);
            hash.Add(Humidity);
            hash.Add(Temperature);
            hash.Add(Pressure);
            hash.Add(MovementCount);
            hash.Add(Timestamp);
            return hash.ToHashCode();
        }
    }



    //EventHubIngestionProcessor {"id": {"int": 278562264213850593437573837145952745064}, "ambient": {"temperature": 22.145392862294102, "humidity": 55.723553390191306}, "machine": {"temperature": 22.145392862294102, "pressure": 981.3503507043997}, "timeCreated": "2020-07-24 08:50:00.549644"}

}
