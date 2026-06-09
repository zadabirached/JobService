namespace JobService.Domain.Entities;

public class SystemOrder
{
    public int Id { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string OrderType { get; set; } = string.Empty;
    public string AreaId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ProgressionRate { get; set; }
    public int Priority { get; set; }
    public bool IsCompleted { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string DeviceRegistryName { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? EstimatedEndTime { get; set; }
    public double? EstimatedRemainingDistance { get; set; }
    public DateTime? UpdatedByOrderTrackingOn { get; set; }
    public string CancellationReason { get; set; } = string.Empty;
    public string CancellationDescription { get; set; } = string.Empty;
    public string? Errors { get; set; }
    public string? OperatingModes { get; set; }
    public string BundleId { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime UpdatedOn { get; set; }

    public ICollection<OrderInstructions> OrderInstructions { get; set; } = [];
}