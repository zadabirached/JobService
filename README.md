# JobService

## Run
dotnet run --project JobService.API

## Configuration
Set the DB connection string via `appsettings.json` or the environment variable:
`ConnectionStrings__JobService`

## Full stack (with ReplayTool)
`docker-compose.yml` in this folder also builds and runs `../ReplayTool` alongside Postgres,
RabbitMQ, Prometheus and Grafana. See [../ReplayTool/README.md](../ReplayTool/README.md) for the
full capture → replay walkthrough.