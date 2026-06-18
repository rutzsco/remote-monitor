using IngestionMeasurementSummary = RemoteMonitor.Services.Ingestion.Model.MeasurementSummary;
using MeasurementSummary = RemoteMonitor.Services.Mesurements.Model.MeasurementSummary;
using TopologyDevice = RemoteMonitor.Services.Topology.Model.Device;
using UiDevice = RemoteMonitor.UI.Models.Device;
using UiDeviceSummary = RemoteMonitor.UI.Models.DeviceSummary;
using UiMeasurementSummary = RemoteMonitor.UI.Models.MeasurementSummary;
using UiNotification = RemoteMonitor.UI.Models.MeasurementNotificationViewModel;

namespace RemoteMonitor.UI.Blazor.Server.Services
{
    public static class MeasurementMapper
    {
        public static UiMeasurementSummary ToUiMeasurement(MeasurementSummary measurement)
        {
            return new UiMeasurementSummary
            {
                id = measurement.Id,
                deviceId = measurement.DeviceId,
                measurementType = measurement.MeasurementType,
                humidity = measurement.Humidity,
                temperature = measurement.Temperature,
                pressure = measurement.Pressure,
                timestamp = measurement.Timestamp
            };
        }

        public static UiNotification ToNotification(MeasurementSummary measurement)
        {
            return new UiNotification
            {
                DeviceId = measurement.DeviceId,
                Temperature = measurement.Temperature,
                Pressure = measurement.Pressure,
                Humidity = measurement.Humidity,
                MovementCount = measurement.MovementCount,
                Timestamp = measurement.Timestamp
            };
        }

        public static MeasurementSummary ToMeasurement(IngestionMeasurementSummary measurement)
        {
            return new MeasurementSummary(
                measurement.DeviceId,
                measurement.MeasurementType,
                measurement.Temperature,
                measurement.Humidity,
                measurement.Pressure,
                measurement.MovementCount,
                measurement.Timestamp);
        }

        public static UiDevice ToUiDevice(TopologyDevice device)
        {
            return new UiDevice
            {
                Id = device.Id,
                Name = device.Name,
                TemperatureOffset = device.TemperatureOffset,
                HumidityOffset = device.HumidityOffset
            };
        }

        public static UiDeviceSummary ToUiDeviceSummary(TopologyDevice device)
        {
            return new UiDeviceSummary
            {
                Id = device.Id,
                DeviceId = device.Id,
                Name = device.Name,
                MeasurementType = "device"
            };
        }

        public static TopologyDevice ToTopologyDevice(UiDevice device)
        {
            return new TopologyDevice
            {
                Id = device.Id,
                Name = device.Name,
                TemperatureOffset = device.TemperatureOffset,
                HumidityOffset = device.HumidityOffset
            };
        }
    }
}