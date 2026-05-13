# Introduction 

Frontend service responsible for ingesting telemetry from different devices. Supports different schemas and maps to common data model.

# Design

![image](https://github.com/rutzsco/RemoteMonitor/assets/50925884/d53af59e-94a7-4360-b14e-112008af7fb7)

# Health Model

## Application

| Failed Requests| Dimension | Threasholds
| ----------- | ----------- | -----------
| Failed Requests | EventHubIngestionProcessor, RuuviMeasurementEndpoint | TBD
| Performance   | EventHubIngestionProcessor, RuuviMeasurementEndpoint | TBD
| Dependancy Failures | MeasurementService(HTTP) | TBD
| Dependancy Performance | MeasurementService(HTTP) | TBD
| Processing Lag | EventHub | TBD

## Infrastructure

| Failed Requests| Dimension | Threasholds
| ----------- | ----------- | -----------
| Function | EventHubIngestionProcessor, RuuviMeasurementEndpoint | TBD
| EventHub   | EventHubIngestionProcessor, RuuviMeasurementEndpoint | TBD
