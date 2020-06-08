using Microsoft.EntityFrameworkCore.Migrations;

namespace PostgresData.Migrations
{
    public partial class _0806v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "SalesFromStockOld",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "SalesFromStockOld");
        }
    }
}
