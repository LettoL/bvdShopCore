using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PostgresData.Migrations
{
    public partial class _1810 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpensesOld",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpenseId = table.Column<int>(nullable: false),
                    ForId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesOld", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldFromSupplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SoldProductId = table.Column<Guid>(nullable: false),
                    SuppliedProductId = table.Column<Guid>(nullable: false),
                    ProcurementCost = table.Column<decimal>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldFromSupplies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpensesOld");

            migrationBuilder.DropTable(
                name: "SoldFromSupplies");
        }
    }
}
