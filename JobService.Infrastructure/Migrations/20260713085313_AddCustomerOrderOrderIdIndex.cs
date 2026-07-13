using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerOrderOrderIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_OrderId",
                table: "CustomerOrders",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerOrders_OrderId",
                table: "CustomerOrders");
        }
    }
}
