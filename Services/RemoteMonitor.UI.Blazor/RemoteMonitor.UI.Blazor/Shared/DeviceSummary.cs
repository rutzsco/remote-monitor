using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteMonitor.UI.Models
{
    public class DeviceSummary
    {
        public string Id { get; set; }

        public string DeviceId { get; set; }

        public string Name { get; set; }

        public string MeasurementType { get; set; }
    }
}
