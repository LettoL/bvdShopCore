using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PostgresData.Migrations
{
    public partial class _0806v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesFromStockOld",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesFromStockOld", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldProductFromStockOld",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(nullable: false),
                    SaleFromStockOldId = table.Column<int>(nullable: false),
                    ProcurementCost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProductFromStockOld", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldProductFromStockOld_SalesFromStockOld_SaleFromStockOldId",
                        column: x => x.SaleFromStockOldId,
                        principalTable: "SalesFromStockOld",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldProductFromStockOld_SaleFromStockOldId",
                table: "SoldProductFromStockOld",
                column: "SaleFromStockOldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldProductFromStockOld");

            migrationBuilder.DropTable(
                name: "SalesFromStockOld");
        }
    }
}
