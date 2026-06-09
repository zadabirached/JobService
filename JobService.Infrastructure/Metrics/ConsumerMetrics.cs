using System.Diagnostics.Metrics;

namespace JobService.Infrastructure.Metrics;

public class ConsumerMetrics
{
    private readonly Counter<long> _eventsConsumedTotal;
    private readonly Histogram<double> _processingDuration;

    public ConsumerMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("JobService");
        _eventsConsumedTotal = meter.CreateCounter<long>(
            "events_consumed_total",
            description: "Total number of events consumed, labelled by topic");
        _processingDuration = meter.CreateHistogram<double>(
            "consumer_processing_duration",
            unit: "ms",
            description: "Consumer processing duration in milliseconds");
    }

    public void RecordEvent(string topic) =>
        _eventsConsumedTotal.Add(1, new KeyValuePair<string, object?>("topic", topic));

    public void RecordDuration(string topic, double milliseconds) =>
        _processingDuration.Record(milliseconds, new KeyValuePair<string, object?>("topic", topic));
}