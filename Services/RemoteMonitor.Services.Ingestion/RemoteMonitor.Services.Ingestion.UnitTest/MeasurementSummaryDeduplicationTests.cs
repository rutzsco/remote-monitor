using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteMonitor.Services.Ingestion.Logic;
using RemoteMonitor.Services.Ingestion.Model;
using System;
using System.Linq;

namespace RemoteMonitor.Services.Ingestion.UnitTest
{
    [TestClass]
    public class MeasurementSummaryDeduplicationTests
    {
        [TestMethod]
        public void Deduplication_Duplicates_Only()
        {
            var dt = DateTime.UtcNow;
            var ms1 = new MeasurementSummary("A", "snapshot", 25, 45, 900, null, dt);
            var ms2 = new MeasurementSummary("A", "snapshot", 25, 45, 900, null, dt);

            var result = MeasurementSummaryDeduplication.Execute(new []{ ms1, ms2});

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Deduplication_Mixed()
        {
            var dt = DateTime.UtcNow;
            var ms1 = new MeasurementSummary("A", "snapshot", 25, 45, 900, null, dt);
            var ms2 = new MeasurementSummary("A", "snapshot", 25, 45, 900, null, dt);
            var ms3 = new MeasurementSummary("B", "snapshot", 25, 45, 900, null, dt);

            var result = MeasurementSummaryDeduplication.Execute(new[] { ms1, ms2, ms3 });

            Assert.AreEqual(2, result.Count());
        }
    }
}
