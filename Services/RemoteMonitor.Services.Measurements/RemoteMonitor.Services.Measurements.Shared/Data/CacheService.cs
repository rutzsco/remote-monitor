using Microsoft.ApplicationInsights;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteMonitor.Services.Measurements.Data
{
    public class CacheService
    {
        public static void Execute(TelemetryClient telemetryClient)
        {
            // Simulate Cache Lookup Dependency
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                Thread.Sleep(10);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                telemetryClient.TrackException(ex);
                throw new Exception("Operation went wrong", ex);
            }
            finally
            {
                timer.Stop();
                telemetryClient.TrackDependency("CacheLookup", "Redis", "", startTime, timer.Elapsed, success);
            }
        }
    }
}
