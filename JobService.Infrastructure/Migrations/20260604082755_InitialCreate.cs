using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    AutomationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AutomationName = table.Column<string>(type: "text", nullable: false),
                    AutomationChainingId = table.Column<int>(type: "integer", nullable: false),
                    AreaId = table.Column<string>(type: "text", nullable: false),
                    FleetId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobType = table.Column<string>(type: "text", nullable: false),
                    JobPriority = table.Column<int>(type: "integer", nullable: false),
                    Tags = table.Column<string[]>(type: "text[]", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    OrderType = table.Column<string>(type: "text", nullable: false),
                    AreaId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ProgressionRate = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceName = table.Column<string>(type: "text", nullable: false),
                    DeviceRegistryName = table.Column<string>(type: "text", nullable: false),
                    DeviceType = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedRemainingDistance = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedByOrderTrackingOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancellationReason = table.Column<string>(type: "text", nullable: false),
                    CancellationDescription = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    Errors = table.Column<string>(type: "jsonb", nullable: true),
                    OperatingModes = table.Column<string>(type: "jsonb", nullable: true),
                    BundleId = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    MissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MissionName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ProgressionRate = table.Column<int>(type: "integer", nullable: false),
                    OrderType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    IsTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceName = table.Column<string>(type: "text", nullable: false),
                    DeviceRegistryName = table.Column<string>(type: "text", nullable: false),
                    DeviceType = table.Column<string>(type: "text", nullable: false),
                    AssignedBy = table.Column<string>(type: "text", nullable: false),
                    AssignmentType = table.Column<string>(type: "text", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    QueuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReassignmentCount = table.Column<int>(type: "integer", nullable: false),
                    ReassignedDeviceName = table.Column<string>(type: "text", nullable: false),
                    ReassignedDeviceRegistryName = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedRemainingDistance = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedByOrderTrackingOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancellationReason = table.Column<string>(type: "text", nullable: false),
                    CancellationDescription = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    Errors = table.Column<string>(type: "jsonb", nullable: true),
                    OperatingModes = table.Column<string>(type: "jsonb", nullable: true),
                    BundleId = table.Column<string>(type: "text", nullable: false),
                    RobotOrderId = table.Column<string>(type: "text", nullable: false),
                    PreviousRobotOrderIds = table.Column<string[]>(type: "text[]", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderInstructions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InstructionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchemaVersion = table.Column<string>(type: "text", nullable: false),
                    ParentInstructionId = table.Column<int>(type: "integer", nullable: true),
                    CustomerOrderId = table.Column<int>(type: "integer", nullable: true),
                    SystemOrderId = table.Column<int>(type: "integer", nullable: true),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    ExecutionSequence = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Reachable = table.Column<bool>(type: "boolean", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateOrderConditions = table.Column<string>(type: "jsonb", nullable: true),
                    WayPoints = table.Column<string>(type: "jsonb", nullable: true),
                    Goal = table.Column<string>(type: "jsonb", nullable: true),
                    Actions = table.Column<string>(type: "jsonb", nullable: true),
                    SystemActions = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderInstructions_CustomerOrders_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalTable: "CustomerOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderInstructions_OrderInstructions_ParentInstructionId",
                        column: x => x.ParentInstructionId,
                        principalTable: "OrderInstructions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderInstructions_SystemOrders_SystemOrderId",
                        column: x => x.SystemOrderId,
                        principalTable: "SystemOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_JobId",
                table: "CustomerOrders",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInstructions_CustomerOrderId",
                table: "OrderInstructions",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInstructions_ParentInstructionId",
                table: "OrderInstructions",
                column: "ParentInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInstructions_SystemOrderId",
                table: "OrderInstructions",
                column: "SystemOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderInstructions");

            migrationBuilder.DropTable(
                name: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "SystemOrders");

            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
