using System.Diagnostics;
using JobService.Domain.Events;
using JobService.Infrastructure.Metrics;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace JobService.Infrastructure.Consumers;

public class OrdersRoutingEventV2Consumer : IConsumer<OrdersRoutingEventV2>
{
    private readonly ILogger<OrdersRoutingEventV2Consumer> _logger;
    private readonly ConsumerMetrics _metrics;

    public OrdersRoutingEventV2Consumer(ILogger<OrdersRoutingEventV2Consumer> logger, ConsumerMetrics metrics)
    {
        _logger = logger;
        _metrics = metrics;
    }

    public Task Consume(ConsumeContext<OrdersRoutingEventV2> context)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation(
            "OrdersRoutingEvent-V2 received on routingResponses/v2 — CorrelationId: {CorrelationId}, AreaId: {AreaId}",
            context.Message.CorrelationId,
            context.Message.AreaId);
        _metrics.RecordEvent("routingResponses/v2");
        _metrics.RecordDuration("routingResponses/v2", sw.Elapsed.TotalMilliseconds);
        return Task.CompletedTask;
    }
}