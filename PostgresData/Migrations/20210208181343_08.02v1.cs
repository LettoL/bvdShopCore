using Microsoft.EntityFrameworkCore.Migrations;

namespace PostgresData.Migrations
{
    public partial class _0802v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryType",
                table: "ScheduledProductDeliveries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryType",
                table: "ScheduledProductDeliveries");
        }
    }
}
