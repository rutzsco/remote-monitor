using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Ingestion.Model
{
    public class EventHubMeasurementMessage
    {
        public EventHubMeasurementTelemetry telemetry { get; set; }
    }

    public class EventHubMeasurementTelemetry
    {
        public decimal humidity { get; set; }

        public decimal temp { get; set; }

        public decimal temperature { get; set; }

        public decimal pressure { get; set; }

    }
}
