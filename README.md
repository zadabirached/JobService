# JobService

## Run
dotnet run --project JobService.API

## Configuration
`appsettings.json` ships with local dev defaults (Postgres `postgres`/`postgres`, RabbitMQ
`guest`/`guest` on `localhost`). To point at real infrastructure, override via environment
variable instead of editing the checked-in file — env vars always win over `appsettings.json`:

| Key | Default | Description |
|-----|---------|--------------|
| `ConnectionStrings__JobService` | `Host=localhost;Port=5432;Database=jobservice;Username=postgres;Password=postgres` | Postgres connection string |
| `RabbitMQ__Host` | `localhost` | Broker host |
| `RabbitMQ__Port` | `5672` | Broker port |
| `RabbitMQ__Username` | `guest` | Broker username |
| `RabbitMQ__Password` | `guest` | Broker password |

Example:

```bash
ConnectionStrings__JobService="Host=my-real-db.example.com;Database=jobservice;Username=app_user;Password=REDACTED" \
RabbitMQ__Host=my-real-broker.example.com \
RabbitMQ__Username=app_user \
RabbitMQ__Password=REDACTED \
dotnet run --project JobService.API
```

### Docker Compose: real credentials via `.env`

When running via `docker compose`, don't edit `docker-compose.yml` to set real credentials — it's
checked into git. Instead, drop a `.env` file next to `docker-compose.yml` (Compose reads it
automatically, and `.env` is already git-ignored, so real credentials never get committed):

```bash
cp .env.example .env
# then edit .env with real values
docker compose up --build
```

`.env.example` documents the available keys (`POSTGRES_DB`, `POSTGRES_USER`,
`POSTGRES_PASSWORD`, `RABBITMQ_USER`, `RABBITMQ_PASSWORD`, `GRAFANA_ADMIN_PASSWORD`). These
values now drive both the actual `postgres`/`rabbitmq` container credentials **and** the
connection strings `jobservice`/`replaytool` use to reach them, so there's a single source of
truth — no more editing multiple places to change a password.

## Full stack (with ReplayTool)
`docker-compose.yml` in this folder also builds and runs `../ReplayTool` alongside Postgres,
RabbitMQ, Prometheus and Grafana. See [../ReplayTool/README.md](../ReplayTool/README.md) for the
full capture → replay walkthrough.