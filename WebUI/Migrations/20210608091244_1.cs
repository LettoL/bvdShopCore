using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebUI.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailableProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoneyWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    CardNumber = table.Column<string>(nullable: true),
                    ForManager = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyWorkers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Debt = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplyHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test1s",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Cost = table.Column<decimal>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    StockAmount = table.Column<int>(nullable: true),
                    PrimeCost = table.Column<decimal>(nullable: true),
                    BookingAmount = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    CategoryTitle = table.Column<string>(nullable: true),
                    ShopId = table.Column<int>(nullable: false),
                    ShopTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Reserv = table.Column<bool>(nullable: false),
                    BookingAmount = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_MoneyWorkers_ShopId",
                        column: x => x.ShopId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_MoneyWorkers_ShopId",
                        column: x => x.ShopId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: true),
                    PartnerId = table.Column<int>(nullable: true),
                    CashSum = table.Column<decimal>(nullable: false),
                    CashlessSum = table.Column<decimal>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    Pay = table.Column<decimal>(nullable: false),
                    Debt = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    forRussian = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_MoneyWorkers_ShopId",
                        column: x => x.ShopId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    AdditionalComment = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    CashSum = table.Column<decimal>(nullable: false),
                    CashlessSum = table.Column<decimal>(nullable: false),
                    PrimeCost = table.Column<decimal>(nullable: false),
                    Margin = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    Payment = table.Column<bool>(nullable: false),
                    ForRussian = table.Column<bool>(nullable: false),
                    ShopId = table.Column<int>(nullable: false),
                    PartnerId = table.Column<int>(nullable: true),
                    SaleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_MoneyWorkers_ShopId",
                        column: x => x.ShopId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartnerProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerProduct_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartnerProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplyProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    SupplyHistoryId = table.Column<int>(nullable: true),
                    TotalAmount = table.Column<int>(nullable: false),
                    RealizationAmount = table.Column<int>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    AdditionalCost = table.Column<decimal>(nullable: false),
                    ProcurementCost = table.Column<decimal>(nullable: false),
                    FinalCost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyProducts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplyProducts_SupplyHistories_SupplyHistoryId",
                        column: x => x.SupplyHistoryId,
                        principalTable: "SupplyHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    BookingId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    Additional = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingProducts_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfoMonies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sum = table.Column<decimal>(nullable: false),
                    MoneyWorkerId = table.Column<int>(nullable: true),
                    SaleId = table.Column<int>(nullable: true),
                    BookingId = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    MoneyOperationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoMonies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoMonies_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfoMonies_MoneyWorkers_MoneyWorkerId",
                        column: x => x.MoneyWorkerId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfoMonies_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleInformations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(nullable: false),
                    MoneyWorkerForIncomeId = table.Column<int>(nullable: true),
                    MoneyWorkerForCashlessIncomeId = table.Column<int>(nullable: true),
                    MoneyWorkerForExpenseId = table.Column<int>(nullable: true),
                    SaleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleInformations_MoneyWorkers_MoneyWorkerForCashlessIncomeId",
                        column: x => x.MoneyWorkerForCashlessIncomeId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleInformations_MoneyWorkers_MoneyWorkerForExpenseId",
                        column: x => x.MoneyWorkerForExpenseId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleInformations_MoneyWorkers_MoneyWorkerForIncomeId",
                        column: x => x.MoneyWorkerForIncomeId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleInformations_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Additional = table.Column<bool>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SalesProducts_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DeferredSupplyProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: true),
                    SupplyProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeferredSupplyProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeferredSupplyProducts_SupplyProducts_SupplyProductId",
                        column: x => x.SupplyProductId,
                        principalTable: "SupplyProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InfoProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: true),
                    SaleId = table.Column<int>(nullable: true),
                    SupplyHistoryId = table.Column<int>(nullable: true),
                    SupplyProductId = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InfoProducts_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InfoProducts_MoneyWorkers_ShopId",
                        column: x => x.ShopId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InfoProducts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InfoProducts_SupplyHistories_SupplyHistoryId",
                        column: x => x.SupplyHistoryId,
                        principalTable: "SupplyHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InfoProducts_SupplyProducts_SupplyProductId",
                        column: x => x.SupplyProductId,
                        principalTable: "SupplyProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductInformations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyProductId = table.Column<int>(nullable: true),
                    SaleId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    FinalCost = table.Column<decimal>(nullable: false),
                    AdditionalCost = table.Column<decimal>(nullable: false),
                    ProcurementCost = table.Column<decimal>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    ForRealization = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInformations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductInformations_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductInformations_SupplyProducts_SupplyProductId",
                        column: x => x.SupplyProductId,
                        principalTable: "SupplyProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoMoneyId = table.Column<int>(nullable: false),
                    ExpenseCategoryId = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseCategories_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Expenses_InfoMonies_InfoMoneyId",
                        column: x => x.InfoMoneyId,
                        principalTable: "InfoMonies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Expenses_MoneyWorkers_ShopId",
                        column: x => x.ShopId,
                        principalTable: "MoneyWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrevInfoMoneyId = table.Column<int>(nullable: false),
                    NextInfoMoneyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyTransfers_InfoMonies_NextInfoMoneyId",
                        column: x => x.NextInfoMoneyId,
                        principalTable: "InfoMonies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MoneyTransfers_InfoMonies_PrevInfoMoneyId",
                        column: x => x.PrevInfoMoneyId,
                        principalTable: "InfoMonies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingProducts_BookingId",
                table: "BookingProducts",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingProducts_ProductId",
                table: "BookingProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PartnerId",
                table: "Bookings",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShopId",
                table: "Bookings",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_DeferredSupplyProducts_SupplyProductId",
                table: "DeferredSupplyProducts",
                column: "SupplyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseCategoryId",
                table: "Expenses",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_InfoMoneyId",
                table: "Expenses",
                column: "InfoMoneyId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ShopId",
                table: "Expenses",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoMonies_BookingId",
                table: "InfoMonies",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoMonies_MoneyWorkerId",
                table: "InfoMonies",
                column: "MoneyWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoMonies_SaleId",
                table: "InfoMonies",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoProducts_ProductId",
                table: "InfoProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoProducts_SaleId",
                table: "InfoProducts",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoProducts_ShopId",
                table: "InfoProducts",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoProducts_SupplierId",
                table: "InfoProducts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoProducts_SupplyHistoryId",
                table: "InfoProducts",
                column: "SupplyHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoProducts_SupplyProductId",
                table: "InfoProducts",
                column: "SupplyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransfers_NextInfoMoneyId",
                table: "MoneyTransfers",
                column: "NextInfoMoneyId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransfers_PrevInfoMoneyId",
                table: "MoneyTransfers",
                column: "PrevInfoMoneyId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProduct_PartnerId",
                table: "PartnerProduct",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProduct_ProductId",
                table: "PartnerProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInformations_ProductId",
                table: "ProductInformations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInformations_SaleId",
                table: "ProductInformations",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInformations_SupplyProductId",
                table: "ProductInformations",
                column: "SupplyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShopId",
                table: "Products",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleInformations_MoneyWorkerForCashlessIncomeId",
                table: "SaleInformations",
                column: "MoneyWorkerForCashlessIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleInformations_MoneyWorkerForExpenseId",
                table: "SaleInformations",
                column: "MoneyWorkerForExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleInformations_MoneyWorkerForIncomeId",
                table: "SaleInformations",
                column: "MoneyWorkerForIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleInformations_SaleId",
                table: "SaleInformations",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PartnerId",
                table: "Sales",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ShopId",
                table: "Sales",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesProducts_ProductId",
                table: "SalesProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesProducts_SaleId",
                table: "SalesProducts",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyProducts_ProductId",
                table: "SupplyProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyProducts_SupplierId",
                table: "SupplyProducts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyProducts_SupplyHistoryId",
                table: "SupplyProducts",
                column: "SupplyHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ShopId",
                table: "Users",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableProducts");

            migrationBuilder.DropTable(
                name: "BookingProducts");

            migrationBuilder.DropTable(
                name: "DeferredSupplyProducts");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "InfoProducts");

            migrationBuilder.DropTable(
                name: "MoneyTransfers");

            migrationBuilder.DropTable(
                name: "PartnerProduct");

            migrationBuilder.DropTable(
                name: "ProductInformations");

            migrationBuilder.DropTable(
                name: "SaleInformations");

            migrationBuilder.DropTable(
                name: "SalesProducts");

            migrationBuilder.DropTable(
                name: "Test1s");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropTable(
                name: "InfoMonies");

            migrationBuilder.DropTable(
                name: "SupplyProducts");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "SupplyHistories");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "MoneyWorkers");
        }
    }
}
