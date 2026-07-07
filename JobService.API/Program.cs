using JobService.Application.Interfaces;
using JobService.Application.UseCases;
using JobService.Infrastructure.Consumers;
using JobService.Infrastructure.Metrics;
using JobService.Infrastructure.Persistence;
using JobService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JobServiceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("JobService")));

builder.Services.AddScoped<ICustomerOrderRepository, CustomerOrderRepository>();
builder.Services.AddScoped<GetCustomerOrdersUseCase>();
builder.Services.AddSingleton<ConsumerMetrics>();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddMeter("JobService")
            .AddPrometheusExporter();
    });

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrdersRoutingEventV2Consumer>();
    x.AddConsumer<AssignmentSolutionV2Consumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        var rabbitPort = builder.Configuration.GetValue<ushort>("RabbitMQ:Port", 5672);
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", rabbitPort, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        cfg.ReceiveEndpoint("OrdersRoutingEvent-V2", e =>
        {
            e.PrefetchCount = 16;
            e.Bind("routingResponses/v2");
            e.ConfigureConsumer<OrdersRoutingEventV2Consumer>(ctx);
        });

        cfg.ReceiveEndpoint("JobServiceAssignmentSolutionV2Queue", e =>
        {
            e.PrefetchCount = 16;
            e.Bind("anytask/solution/v2");
            e.ConfigureConsumer<AssignmentSolutionV2Consumer>(ctx);
        });
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JobServiceDbContext>();
    db.Database.Migrate();
}

app.MapPrometheusScrapingEndpoint();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/customer-orders", async (string orderIds, Guid tenantId, GetCustomerOrdersUseCase useCase) =>
{
    var ids = orderIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
    var orders = await useCase.ExecuteAsync(ids, tenantId);
    return Results.Ok(orders);
});

app.Run();

public partial class Program { }