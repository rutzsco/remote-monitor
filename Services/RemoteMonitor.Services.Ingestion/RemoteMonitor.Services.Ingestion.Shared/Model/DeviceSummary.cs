using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Ingestion.Model
{
    public class DeviceSummary
    {
        public DeviceSummary(string deviceId, string templateId, string fanSpeed)
        {
            DeviceId = deviceId;
            TemplateId = templateId;
            FanSpeed = fanSpeed;
        }

        public string DeviceId { get; set; }
        public string TemplateId { get; set; }

        public string FanSpeed { get; set; }
    }
}
