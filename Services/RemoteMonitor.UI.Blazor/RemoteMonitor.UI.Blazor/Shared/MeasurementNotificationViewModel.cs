using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.UI.Models
{
    public class MeasurementNotificationViewModel
    {
        public string DeviceId { get; set; }
        public decimal Temperature { get; set; }
        public decimal Pressure { get; set; }
        public decimal Humidity { get; set; }
        public int? MovementCount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
