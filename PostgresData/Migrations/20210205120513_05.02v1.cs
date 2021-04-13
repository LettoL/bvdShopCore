using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PostgresData.Migrations
{
    public partial class _0502v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduledDeliveries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierId = table.Column<int>(nullable: false),
                    DepositedSum = table.Column<decimal>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledDeliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledProductDeliveries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    ProcurementCost = table.Column<decimal>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ShopId = table.Column<int>(nullable: false),
                    ScheduledDeliveryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledProductDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledProductDeliveries_ScheduledDeliveries_ScheduledDel~",
                        column: x => x.ScheduledDeliveryId,
                        principalTable: "ScheduledDeliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledProductDeliveries_ScheduledDeliveryId",
                table: "ScheduledProductDeliveries",
                column: "ScheduledDeliveryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledProductDeliveries");

            migrationBuilder.DropTable(
                name: "ScheduledDeliveries");
        }
    }
}
