namespace JobService.Domain.Entities;

public class CustomerOrder
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
    public string OrderId { get; set; } = string.Empty;
    public Guid MissionId { get; set; }
    public string MissionName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ProgressionRate { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public bool IsTemplate { get; set; }
    public bool IsCompleted { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string DeviceRegistryName { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string AssignedBy { get; set; } = string.Empty;
    public string AssignmentType { get; set; } = string.Empty;
    public DateTime? AssignedOn { get; set; }
    public DateTime? QueuedAt { get; set; }
    public int ReassignmentCount { get; set; }
    public string ReassignedDeviceName { get; set; } = string.Empty;
    public string ReassignedDeviceRegistryName { get; set; } = string.Empty;
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
    public string RobotOrderId { get; set; } = string.Empty;
    public string[] PreviousRobotOrderIds { get; set; } = [];
    public Guid TenantId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime UpdatedOn { get; set; }

    public ICollection<OrderInstructions> OrderInstructions { get; set; } = [];
}