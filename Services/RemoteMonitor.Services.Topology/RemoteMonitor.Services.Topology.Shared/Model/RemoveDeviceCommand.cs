using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteMonitor.Services.Topology.Model
{
    public class RemoveDeviceCommand
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
