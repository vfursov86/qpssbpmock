# qpssbpmock

## Description
Implements a response logic from Fast Transfers service.
Project based on ASP.NET Core Web API template
and exposes API with three Controllers.
In general, this is an API server with advanced logging 
and many of performance build-in metrics. 
Viktor Fursov. 2024.

This mock exposes the following endpoints (grouped by Controllers):

1. TransfersController
    - /c2c/transfers/init
    - /c2c/transfers/confirm
    - /c2c/transfers/CheckReceiver
    - /c2c/transfers/participantList
2. IntegratioController
    - /c2c/integration/CheckReceiver
3. FtsController
    - /c2c/fts/banklist
    - / 

## Rinning options
While local deploying you use the launchSettins.json, and appsettings.Development.json configuration files.
In production deployment you using appsettings.json.
If URL and port are not provided, the application runs at localhost:5055.
Be careful about url while a production deployment!

## Logging
Default logging is disabled and log4net used instead.

## Response time
You set the mock's response time with "MockResponseTime" parameter in appsettings.json
ans application.Development.json.

## Metrics
Full of necessary performance metrics provided by Prometheus exporter,
and available at the /metrics endpoint.