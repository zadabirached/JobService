using System.Diagnostics;
using JobService.Domain.Events;
using JobService.Infrastructure.Metrics;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace JobService.Infrastructure.Consumers;

public class AssignmentSolutionV2Consumer : IConsumer<AssignmentSolutionV2Event>
{
    private readonly ILogger<AssignmentSolutionV2Consumer> _logger;
    private readonly ConsumerMetrics _metrics;

    public AssignmentSolutionV2Consumer(ILogger<AssignmentSolutionV2Consumer> logger, ConsumerMetrics metrics)
    {
        _logger = logger;
        _metrics = metrics;
    }

    public Task Consume(ConsumeContext<AssignmentSolutionV2Event> context)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation(
            "JobServiceAssignmentSolutionV2Queue received on anytask/solution/v2 — AreaId: {AreaId}",
            context.Message.AreaId);
        _metrics.RecordEvent("anytask/solution/v2");
        _metrics.RecordDuration("anytask/solution/v2", sw.Elapsed.TotalMilliseconds);
        return Task.CompletedTask;
    }
}