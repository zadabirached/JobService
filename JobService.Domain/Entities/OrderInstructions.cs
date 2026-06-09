namespace JobService.Domain.Entities;

public class OrderInstructions
{
    public int Id { get; set; }
    public Guid InstructionId { get; set; }
    public string SchemaVersion { get; set; } = string.Empty;
    public int? ParentInstructionId { get; set; }
    public OrderInstructions? ParentInstruction { get; set; }
    public int? CustomerOrderId { get; set; }
    public CustomerOrder? CustomerOrder { get; set; }
    public int? SystemOrderId { get; set; }
    public SystemOrder? SystemOrder { get; set; }
    public int Sequence { get; set; }
    public int ExecutionSequence { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Reachable { get; set; }
    public bool Completed { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? UpdateOrderConditions { get; set; }
    public string? WayPoints { get; set; }
    public string? Goal { get; set; }
    public string? Actions { get; set; }
    public string? SystemActions { get; set; }

    public ICollection<OrderInstructions> Children { get; set; } = [];
}