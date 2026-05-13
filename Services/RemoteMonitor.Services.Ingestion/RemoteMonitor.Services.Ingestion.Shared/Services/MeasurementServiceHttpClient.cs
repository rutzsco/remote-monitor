using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RemoteMonitor.Services.Ingestion.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMonitor.Services.Ingestion.Shared.Services
{
    public class MeasurementServiceHttpClient : IMeasurementService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessKey;
        private readonly string _serviceUrl;

        public MeasurementServiceHttpClient(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _serviceUrl = config["MeasurementServiceUrl"];
            _accessKey = config["MeasurementServiceKey"];
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task AddMeasurements(IEnumerable<MeasurementSummary> measurements)
        {
            var url = $"{_serviceUrl}/api/AddMesurementsEndpoint?code={_accessKey}";
            var response = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(measurements), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("An error occured invoking measurement service.");
            }
        }
    }
}
