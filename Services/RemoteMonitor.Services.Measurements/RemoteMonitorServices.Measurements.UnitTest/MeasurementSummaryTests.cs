using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteMonitor.Services.Mesurements.Model;
using System;
using System.Text.Json;

namespace SmartHome.Services.Measurements.UnitTest
{
    [TestClass]
    public class MeasurementSummaryTests
    {
        [TestMethod]
        public void MeasurementSummaryApplyCalibrationTest()
        {
            var ms = new MeasurementSummary("ID", "summary", (decimal)55.5, (decimal)66.6, (decimal)999.4, 0, DateTime.Now);
            ms.ApplyCalibration(-5, -5);

            Assert.AreEqual(Math.Round(ms.Temperature, 2), (decimal)52.72);
            Assert.AreEqual(ms.Humidity, (decimal)61.6);

            
        }

        [TestMethod]
        public void Deserializer()
        {
            //var payload = "{\"id\":\"f7be97b8-8633-4eac-b2d9-19b5f9a58b3d\",\"MeasurementType\":\"summary\",\"DeviceId\":\"f7be97b8-8633-4eac-b2d9-19b5f9a58b3d\",\"Humidity\":26.0,\"HumidityRaw\":26.0,\"TemperatureRaw\":18.77,\"Temperature\":18.77,\"Pressure\":985.79,\"AccelerometerX\":0,\"AccelerometerY\":0,\"AccelerometerZ\":0,\"GyroscopeX\":0,\"GyroscopeY\":0,\"GyroscopeZ\":0,\"Timestamp\":\"2022-03-02T21:44:34Z\"}";
            //var msdto = JsonSerializer.Deserialize<MeasurementSummary>(payload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });
            //Assert.AreEqual("f7be97b8-8633-4eac-b2d9-19b5f9a58b3d", msdto.Id);

            //var e = new EventGridEvent("ExampleEventSubject", "Example.EventType", "1.0", new MeasurementSummary("ID", "summary", (decimal)55.5, (decimal)66.6, (decimal)999.4, 0, DateTime.Now));
            //var msdto2 = JsonSerializer.Deserialize<MeasurementSummary>(e.Data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //Assert.AreEqual("ID", msdto2.Id);
        }
    }
}
