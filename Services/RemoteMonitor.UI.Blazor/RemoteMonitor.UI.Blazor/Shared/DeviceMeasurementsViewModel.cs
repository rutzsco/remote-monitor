using RemoteMonitor.UI.Blazor.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteMonitor.UI.Models
{
    public class DeviceMeasurementsViewModel
    {
        public static DeviceMeasurementsViewModel CreateFromMeasurements(IEnumerable<MeasurementSummary> measurements)
        {
            decimal minTemp = 0;
            decimal maxTemp = 0;
            var dmvm = new DeviceMeasurementsViewModel();

            if (measurements.Any())
            {
                var ordered = measurements.OrderByDescending(x => x.timestamp).ToArray();
                var last24 = ordered.Where(x => x.timestamp >= DateTime.Now.AddDays(-1)).ToArray();

                if (last24.Any())
                {
                    maxTemp = last24.Max(x => x.temperatureF);
                    minTemp = last24.Min(x => x.temperatureF);
                }

                var current = ordered.First();
                dmvm.Temperature = current.temperatureF;
                dmvm.Humidity = current.humidity;
                dmvm.Pressure = current.pressure;
                dmvm.MinTemperature = minTemp;
                dmvm.MaxTemperature = maxTemp;
                dmvm.HistoricalMeasurements = measurements ?? throw new ArgumentNullException(nameof(measurements));
                dmvm.ConnectionStatus = EvaluateConnectionStatus(current);
            }
            else
            {
                dmvm.ConnectionStatus = "Disconnected";
            }

            return dmvm;
        }

        public DeviceMeasurementsViewModel()
        {
        }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }

        public decimal Pressure { get; set; }

        public decimal MinTemperature { get; set; }

        public decimal MaxTemperature { get; set; }

        public string ConnectionStatus { get; set; }

        public IEnumerable<MeasurementSummary> HistoricalMeasurements { get; set; }

        private static string EvaluateConnectionStatus(MeasurementSummary measurementSummary)
        {
            var now = DateTime.UtcNow - measurementSummary.timestamp;
            if (now.TotalMinutes > 10)
            {
                return "Disconnected";
            }

            return "Connected";
        }
    }
}
