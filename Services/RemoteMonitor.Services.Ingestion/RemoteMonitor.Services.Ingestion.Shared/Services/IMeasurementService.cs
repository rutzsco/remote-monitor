using RemoteMonitor.Services.Ingestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMonitor.Services.Ingestion.Shared.Services
{
    public interface IMeasurementService
    {
        Task AddMeasurements(IEnumerable<MeasurementSummary> measurements);
    }
}
