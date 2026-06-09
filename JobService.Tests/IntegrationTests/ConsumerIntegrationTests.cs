using JobService.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace JobService.Tests.IntegrationTests;

[Trait("Category", "Integration")]
public class ConsumerIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder().WithImage("postgres:latest").Build();
    private readonly RabbitMqContainer _rabbitMq = new RabbitMqBuilder().WithImage("rabbitmq:management").WithUsername("testuser").WithPassword("testpass").Build();
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        await Task.WhenAll(_postgres.StartAsync(), _rabbitMq.StartAsync());

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ConnectionStrings:JobService", _postgres.GetConnectionString());
                builder.UseSetting("RabbitMQ:Host", _rabbitMq.Hostname);
                builder.UseSetting("RabbitMQ:Port", _rabbitMq.GetMappedPublicPort(5672).ToString());
                builder.UseSetting("RabbitMQ:Username", "testuser");
                builder.UseSetting("RabbitMQ:Password", "testpass");
            });

        _client = _factory.CreateClient();

        // Give consumers time to connect and register their queues/exchanges
        await Task.Delay(TimeSpan.FromSeconds(2));
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
        await Task.WhenAll(_postgres.StopAsync(), _rabbitMq.StopAsync());
    }

    [Fact]
    public async Task RoutingEventConsumer_IncrementsMetric_WhenMessageReceived()
    {
        var message = new OrdersRoutingEventV2
        {
            AreaId = "test-area",
            TenantId = Guid.NewGuid().ToString(),
            CorrelationId = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow,
            RobotRoutes = new Dictionary<string, OrdersRoutingEventV2.Route>(),
        };

        var bus = _factory.Services.GetRequiredService<IBus>();
        var endpoint = await bus.GetSendEndpoint(new Uri("queue:OrdersRoutingEvent-V2"));
        await endpoint.Send(message);

        var metricIncremented = await PollForMetricAsync("events_consumed_total", "routingResponses/v2");
        metricIncremented.Should().BeTrue("the routing event consumer should have received the message and incremented the metric");
    }

    [Fact]
    public async Task AssignmentSolutionConsumer_IncrementsMetric_WhenMessageReceived()
    {
        var message = new AssignmentSolutionV2Event
        {
            TenantId = Guid.NewGuid().ToString(),
            AreaId = "test-area",
            Timestamp = DateTime.UtcNow,
            Decisions = new List<Decision>(),
        };

        var bus = _factory.Services.GetRequiredService<IBus>();
        var endpoint = await bus.GetSendEndpoint(new Uri("queue:JobServiceAssignmentSolutionV2Queue"));
        await endpoint.Send(message);

        var metricIncremented = await PollForMetricAsync("events_consumed_total", "anytask/solution/v2");
        metricIncremented.Should().BeTrue("the assignment solution consumer should have received the message and incremented the metric");
    }

    private async Task<bool> PollForMetricAsync(string metricName, string topic, int timeoutSeconds = 10)
    {
        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
        while (DateTime.UtcNow < deadline)
        {
            var metricsText = await _client.GetStringAsync("/metrics");
            if (metricsText.Contains(metricName) && metricsText.Contains(topic))
                return true;
            await Task.Delay(TimeSpan.FromMilliseconds(300));
        }
        return false;
    }
}
