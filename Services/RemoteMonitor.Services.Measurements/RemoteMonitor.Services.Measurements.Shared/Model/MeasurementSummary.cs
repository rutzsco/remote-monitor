using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Mesurements.Model
{
    public class MeasurementSummary
    {
        public static string MeasurementTypeSummary = "summary";
        public static string MeasurementTypeAggregation = "aggregation";

        public MeasurementSummary(string deviceId, string measurementType, decimal temperature, decimal humidity, decimal pressure, int? movementCount, DateTime timestamp)
        {
            DeviceId = deviceId;
            MeasurementType = measurementType;

            Humidity = decimal.Round(humidity, 2);
            HumidityRaw = decimal.Round(humidity, 2);

            TemperatureRaw = decimal.Round(temperature, 2);
            Temperature = decimal.Round(temperature, 2);

            Pressure = pressure;
            MovementCount = movementCount;

            Timestamp = timestamp;

            if (measurementType.ToLower() == MeasurementTypeSummary)
            {
                Id = deviceId;
            }
            else if (measurementType.ToLower() == MeasurementTypeAggregation)
            {
                Id = $"{deviceId}-{Timestamp.Year}-{Timestamp.Month}-{Timestamp.Day}-{Timestamp.Hour}";
            }
            else
                Id = Guid.NewGuid().ToString();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        public string MeasurementType { get; set; }

        public string DeviceId { get; set; }

        public decimal Humidity { get; set; }

        public decimal HumidityRaw { get; set; }

        public decimal TemperatureRaw { get; set; }

        public decimal Temperature { get; set; }

        public decimal Pressure { get; set; }

        [JsonProperty("movementCount")]
        public int? MovementCount { get; set; }

        public DateTime Timestamp { get; set; }

        public string GetAggregationCode1Hour()
        {
            var ts = Timestamp.ToUniversalTime();
            return $"{ts.Year}-{ts.Month}-{ts.Day}-{ts.Hour}";
        }

        public void ApplyCalibration(int fahrenheitOff, int humidityOff)
        {
            var fahrenheit = (TemperatureRaw * 9) / 5 + 32;
            fahrenheit = fahrenheit + fahrenheitOff;
            var celcius = (fahrenheit - 32) * 5 / 9;

            Temperature = Math.Round(celcius, 2);
            Humidity = Humidity + humidityOff;
        }
    }
}
