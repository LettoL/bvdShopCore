using Microsoft.EntityFrameworkCore.Migrations;

namespace PostgresData.Migrations
{
    public partial class _0303v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledProductDeliveries_ScheduledDeliveries_ScheduledDel~",
                table: "ScheduledProductDeliveries");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduledDeliveryId",
                table: "ScheduledProductDeliveries",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledProductDeliveries_ScheduledDeliveries_ScheduledDel~",
                table: "ScheduledProductDeliveries",
                column: "ScheduledDeliveryId",
                principalTable: "ScheduledDeliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledProductDeliveries_ScheduledDeliveries_ScheduledDel~",
                table: "ScheduledProductDeliveries");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduledDeliveryId",
                table: "ScheduledProductDeliveries",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledProductDeliveries_ScheduledDeliveries_ScheduledDel~",
                table: "ScheduledProductDeliveries",
                column: "ScheduledDeliveryId",
                principalTable: "ScheduledDeliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
