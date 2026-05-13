using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RemoteMonitor.Services.Ingestion.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMonitor.Services.Ingestion.Shared.Logic
{
    public class IotDevKitTelemetryProcessor
    {
        public static MeasurementSummary Process(EventData eventData, ILogger log)
        {
            var messageBodyString = Encoding.UTF8.GetString(eventData.EventBody);
            var message = JsonConvert.DeserializeObject<EventHubMeasurementMessage>(messageBodyString);

            log.LogInformation($"EventHubIngestionProcessor MessagePayload:  {messageBodyString}");

            var deviceId = (string)eventData.Properties["iotcentral-device-id"];
            var enqueuedTime = DateTime.Parse(eventData.SystemProperties["x-opt-enqueued-time"].ToString()).ToUniversalTime();

            var measurementSummary = new MeasurementSummary(deviceId, "snapshot", message.telemetry.temp, message.telemetry.humidity, message.telemetry.pressure, null, enqueuedTime);
 
            log.LogInformation($"EventHubIngestionProcessor Measurement: {deviceId} - {enqueuedTime}");

            return measurementSummary;
        }
    }
}
