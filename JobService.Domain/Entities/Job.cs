namespace JobService.Domain.Entities;

public class Job
{
    public int Id { get; set; }
    public Guid JobId { get; set; }
    public Guid AutomationId { get; set; }
    public string AutomationName { get; set; } = string.Empty;
    public int AutomationChainingId { get; set; }
    public string AreaId { get; set; } = string.Empty;
    public Guid FleetId { get; set; }
    public Guid TenantId { get; set; }
    public string JobType { get; set; } = string.Empty;
    public int JobPriority { get; set; }
    public string[] Tags { get; set; } = [];
    public bool IsCompleted { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime UpdatedOn { get; set; }
    public string? Notes { get; set; }

    public ICollection<CustomerOrder> CustomerOrders { get; set; } = [];
}