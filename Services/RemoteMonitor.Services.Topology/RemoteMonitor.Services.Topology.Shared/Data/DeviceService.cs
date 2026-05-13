using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RemoteMonitor.Services.Topology.Model;
using Microsoft.Azure.Cosmos;


namespace RemoteMonitor.Services.Topology.Data
{
    public class DeviceService
    {
        private readonly CosmosClient _cosmosClient;

        public DeviceService(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<IEnumerable<Device>> GetDevicesBySiteAsync()
        {
            var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Measurements");
            var container = await db.Database.CreateContainerIfNotExistsAsync("DeviceInstance", "/id");

            List<Device> devices = new();
            using (FeedIterator<Device> setIterator = container.Container.GetItemQueryIterator<Device>(requestOptions: new QueryRequestOptions { MaxItemCount = 100 }))
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (Device item in await setIterator.ReadNextAsync())
                    {
                        devices.Add(item);
                    }
                }
            }

            return devices;
        }

        public async Task DeleteDevice(string id)
        {
            var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Measurements");
            var container = await db.Database.CreateContainerIfNotExistsAsync("DeviceInstance", "/id");

            ItemResponse<Device> response = await container.Container.DeleteItemAsync<Device>(partitionKey: new PartitionKey(id), id: id);
        }

        public async Task<Device> GetDeviceByIdAsync(string id)
        {
            var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Measurements");
            var container = await db.Database.CreateContainerIfNotExistsAsync("DeviceInstance", "/id");

            ItemResponse<Device> response = await container.Container.ReadItemAsync<Device>( partitionKey: new PartitionKey(id), id: id);
            return (Device)response;
        }

        public async Task AddUpdateDevice(Device device)
        {
            var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Measurements");
            var container = await db.Database.CreateContainerIfNotExistsAsync("DeviceInstance", "/id");

            ItemResponse<Device> response = await container.Container.UpsertItemAsync<Device>(device, partitionKey: new PartitionKey(device.Id));
        }
    }
}