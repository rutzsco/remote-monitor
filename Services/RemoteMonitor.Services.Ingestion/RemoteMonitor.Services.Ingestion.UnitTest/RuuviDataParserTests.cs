using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteMonitor.Services.Ingestion.Logic;
using RemoteMonitor.Services.Ingestion.Model;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RemoteMonitor.Services.Ingestion.UnitTest
{
    [TestClass]
    public class RuuviDataParserTests
    {
        [TestMethod]
        public void Parse()
        {
            var r = new RuuviDataParser(null);
            r.UnpackAllData("0201061BFF9904050EA63F21BC2D0014FFC803D8C2566A4228C312AD5C4DE7");
            var t = r.temperature;
            Assert.AreEqual("C312AD5C4DE7", r.MAC);
            Assert.AreEqual("106", r.moveCounter);
        }

        [TestMethod]
        public void Deserialize()
        {
            var payload = ReadResourceFile("RemoteMonitor.Services.Ingestion.UnitTest.payload.json");
            var dto = RuuviPayloadDataModel.CreateFromString(payload);
            var tag = dto.data.tags.First();

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tag.Value.timestamp); //from start epoch time
            Assert.AreEqual(2022, dt.Year);
        }

        private string ReadResourceFile(string filename)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            using (var stream = thisAssembly.GetManifestResourceStream(filename))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
