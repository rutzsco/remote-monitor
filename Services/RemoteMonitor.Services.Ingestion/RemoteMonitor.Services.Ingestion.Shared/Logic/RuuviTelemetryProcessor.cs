using Azure.Messaging.EventHubs;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RemoteMonitor.Services.Ingestion.Logic;
using RemoteMonitor.Services.Ingestion.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMonitor.Services.Ingestion.Shared.Logic
{
    public class RuuviTelemetryProcessor
    {
        public static IEnumerable<MeasurementSummary> Process(string messageBodyString, ILogger logger)
        {
            logger.LogInformation($"RuuviMeasurementEndpoint - START: {messageBodyString}");     
            var dto = RuuviPayloadDataModel.CreateFromString(messageBodyString);
            return Process(dto.data, logger);
        }
        public static IEnumerable<MeasurementSummary> ProcessCloudEvent(string messageBodyString, ILogger logger)
        {
            logger.LogInformation($"RuuviMeasurementEndpoint - START: {messageBodyString}");
            var data = RuuviDataModel.CreateFromString(messageBodyString);
            return Process(data, logger);
        }

        public static IEnumerable<MeasurementSummary> Process(RuuviDataModel data, ILogger logger)
        {
            var measurements = new List<MeasurementSummary>();
            foreach (var deviceId in data.tags.Keys)
            {
                try
                {
                    var tag = data.tags[deviceId];
                    var dp = new RuuviDataParser(logger);
                    dp.UnpackAllData(tag.data);
                    logger.LogInformation($"RuuviMeasurementEndpoint Measurement: {dp.temperature} - {dp.humidity} - {dp.pressure}");

                    var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tag.timestamp);
                    var measurementSummary = new MeasurementSummary(deviceId, "snapshot", decimal.Parse(dp.temperature), decimal.Parse(dp.humidity), decimal.Parse(dp.pressure), int.Parse(dp.moveCounter), dt);
                    measurements.Add(measurementSummary);

                    logger.LogInformation($"RuuviMeasurementEndpoint Measurement: {deviceId} - {dt}");
                }
                catch(Exception ex) 
                {
                    logger.LogError($"RuuviMeasurementEndpoint - {ex.Message}", ex);
                }
            }

            logger.LogInformation($"RuuviMeasurementEndpoint - COMPLETE");
            return measurements;
        }
    }
}
