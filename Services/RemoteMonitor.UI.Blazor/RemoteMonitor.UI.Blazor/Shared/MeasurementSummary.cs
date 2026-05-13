using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteMonitor.UI.Models
{
 
    public class MeasurementSummary
    {
        public MeasurementSummary()
        {
        }

        public string id { get; set; }

        public string measurementType { get; set; }

        public string deviceId { get; set; }

        public string temperatureDescription { get { return $"{temperatureF}({temperature})"; } }

        public Decimal humidity { get; set; }

        public Decimal temperature { get; set; }

        public Decimal temperatureF 
        {
            get
            { 
                return (temperature * 9) / 5 + 32;
            } 
        }

        public Decimal pressure { get; set; }

        public DateTime timestamp { get; set; }
    }
}
