
using RemoteMonitor.Services.Ingestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteMonitor.Services.Ingestion.Logic
{
    public static class MeasurementSummaryDeduplication
    {
        public static IEnumerable<MeasurementSummary> Execute(IEnumerable<MeasurementSummary> items)
        {
            return items.Distinct();
        }
    }
}
