using Microsoft.EntityFrameworkCore.Migrations;

namespace PostgresData.Migrations
{
    public partial class _2107v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncompleteProducts_Products_ProductId",
                table: "IncompleteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_IncompleteProducts_Shops_ShopId",
                table: "IncompleteProducts");

            migrationBuilder.DropIndex(
                name: "IX_IncompleteProducts_ProductId",
                table: "IncompleteProducts");

            migrationBuilder.DropIndex(
                name: "IX_IncompleteProducts_ShopId",
                table: "IncompleteProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IncompleteProducts_ProductId",
                table: "IncompleteProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_IncompleteProducts_ShopId",
                table: "IncompleteProducts",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncompleteProducts_Products_ProductId",
                table: "IncompleteProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncompleteProducts_Shops_ShopId",
                table: "IncompleteProducts",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
