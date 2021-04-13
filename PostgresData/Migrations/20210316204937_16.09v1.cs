using Microsoft.EntityFrameworkCore.Migrations;

namespace PostgresData.Migrations
{
    public partial class _1609v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplyProductId",
                table: "ScheduledProductDeliveries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplyProductId",
                table: "ScheduledProductDeliveries");
        }
    }
}
