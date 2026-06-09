using JobService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobService.Infrastructure.Persistence;

public class JobServiceDbContext : DbContext
{
    public JobServiceDbContext(DbContextOptions<JobServiceDbContext> options) : base(options) { }

    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<CustomerOrder> CustomerOrders => Set<CustomerOrder>();
    public DbSet<SystemOrder> SystemOrders => Set<SystemOrder>();
    public DbSet<OrderInstructions> OrderInstructions => Set<OrderInstructions>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Tags).HasColumnType("text[]");
            e.Property(x => x.CreatedOn).HasColumnType("timestamptz");
            e.Property(x => x.UpdatedOn).HasColumnType("timestamptz");
        });

        modelBuilder.Entity<CustomerOrder>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.OrderType).HasMaxLength(21);
            e.Property(x => x.CancellationDescription).HasMaxLength(240);
            e.Property(x => x.PreviousRobotOrderIds).HasColumnType("text[]");
            e.Property(x => x.Errors).HasColumnType("jsonb");
            e.Property(x => x.OperatingModes).HasColumnType("jsonb");
            e.HasOne(x => x.Job).WithMany(x => x.CustomerOrders).HasForeignKey(x => x.JobId);
        });

        modelBuilder.Entity<SystemOrder>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.CancellationDescription).HasMaxLength(240);
            e.Property(x => x.Errors).HasColumnType("jsonb");
            e.Property(x => x.OperatingModes).HasColumnType("jsonb");
        });

        modelBuilder.Entity<OrderInstructions>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.UpdateOrderConditions).HasColumnType("jsonb");
            e.Property(x => x.WayPoints).HasColumnType("jsonb");
            e.Property(x => x.Goal).HasColumnType("jsonb");
            e.Property(x => x.Actions).HasColumnType("jsonb");
            e.Property(x => x.SystemActions).HasColumnType("jsonb");
            e.HasOne(x => x.CustomerOrder).WithMany(x => x.OrderInstructions).HasForeignKey(x => x.CustomerOrderId);
            e.HasOne(x => x.SystemOrder).WithMany(x => x.OrderInstructions).HasForeignKey(x => x.SystemOrderId);
            e.HasOne(x => x.ParentInstruction).WithMany(x => x.Children).HasForeignKey(x => x.ParentInstructionId);
        });
    }
}