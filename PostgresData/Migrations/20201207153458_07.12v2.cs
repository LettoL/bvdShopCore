using Microsoft.EntityFrameworkCore.Migrations;

namespace PostgresData.Migrations
{
    public partial class _0712v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ManagerPayments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ManagerPayments");
        }
    }
}
