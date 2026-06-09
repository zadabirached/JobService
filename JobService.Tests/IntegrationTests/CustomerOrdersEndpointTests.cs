using System.Net;
using System.Net.Http.Json;
using JobService.Domain.Entities;
using JobService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace JobService.Tests.IntegrationTests;

[Trait("Category", "Integration")]
public class CustomerOrdersEndpointTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder().WithImage("postgres:latest").Build();
    private readonly RabbitMqContainer _rabbitMq = new RabbitMqBuilder().WithImage("rabbitmq:management").Build();
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
                builder.UseSetting("RabbitMQ:Username", "guest");
                builder.UseSetting("RabbitMQ:Password", "guest");
                builder.ConfigureServices(services =>
                {
                    // Remove MassTransit hosted service — HTTP endpoint tests don't need the broker
                    var toRemove = services.Where(d => d.ServiceType == typeof(IHostedService)).ToList();
                    foreach (var d in toRemove) services.Remove(d);
                });
            });

        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
        await Task.WhenAll(_postgres.StopAsync(), _rabbitMq.StopAsync());
    }

    [Fact]
    public async Task GetCustomerOrders_ReturnsMatchingOrders_FullStack()
    {
        var tenantId = Guid.NewGuid();
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<JobServiceDbContext>();
            var job = new Job
            {
                JobId = Guid.NewGuid(),
                AutomationId = Guid.NewGuid(),
                AutomationName = "Integration Test Job",
                AreaId = "area-1",
                FleetId = Guid.NewGuid(),
                TenantId = tenantId,
                JobType = "Standard",
                CreatedBy = "test",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "test",
                UpdatedOn = DateTime.UtcNow,
            };
            db.Jobs.Add(job);
            await db.SaveChangesAsync();

            db.CustomerOrders.Add(new CustomerOrder
            {
                JobId = job.Id,
                OrderId = "integration-order-1",
                TenantId = tenantId,
                Status = "Active",
                CreatedBy = "test",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "test",
                UpdatedOn = DateTime.UtcNow,
            });
            await db.SaveChangesAsync();
        }

        var response = await _client.GetAsync($"/customer-orders?orderIds=integration-order-1&tenantId={tenantId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.Content.ReadFromJsonAsync<List<CustomerOrder>>();
        orders.Should().HaveCount(1);
        orders![0].OrderId.Should().Be("integration-order-1");
        orders[0].TenantId.Should().Be(tenantId);
    }

    [Fact]
    public async Task GetCustomerOrders_ReturnsEmpty_ForUnknownOrderIds()
    {
        var response = await _client.GetAsync($"/customer-orders?orderIds=nonexistent-order&tenantId={Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.Content.ReadFromJsonAsync<List<CustomerOrder>>();
        orders.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCustomerOrders_ReturnsEmpty_WhenTenantDoesNotMatch()
    {
        var ownerTenant = Guid.NewGuid();
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<JobServiceDbContext>();
            var job = new Job
            {
                JobId = Guid.NewGuid(),
                AutomationId = Guid.NewGuid(),
                AutomationName = "Tenant Scoping Test",
                AreaId = "area-1",
                FleetId = Guid.NewGuid(),
                TenantId = ownerTenant,
                JobType = "Standard",
                CreatedBy = "test",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "test",
                UpdatedOn = DateTime.UtcNow,
            };
            db.Jobs.Add(job);
            await db.SaveChangesAsync();

            db.CustomerOrders.Add(new CustomerOrder
            {
                JobId = job.Id,
                OrderId = "tenant-scoped-order",
                TenantId = ownerTenant,
                Status = "Active",
                CreatedBy = "test",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "test",
                UpdatedOn = DateTime.UtcNow,
            });
            await db.SaveChangesAsync();
        }

        var differentTenant = Guid.NewGuid();
        var response = await _client.GetAsync($"/customer-orders?orderIds=tenant-scoped-order&tenantId={differentTenant}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.Content.ReadFromJsonAsync<List<CustomerOrder>>();
        orders.Should().BeEmpty();
    }
}
