﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebUI.Migrations
{
    [DbContext(typeof(ShopContext))]
    [Migration("20210608091244_1")]
    partial class _1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("CashSum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CashlessSum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PartnerId")
                        .HasColumnType("int");

                    b.Property<decimal>("Pay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ShopId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("Sum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("forRussian")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("PartnerId");

                    b.HasIndex("ShopId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Data.Entities.BookingProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Additional")
                        .HasColumnType("bit");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("ProductId");

                    b.ToTable("BookingProducts");
                });

            modelBuilder.Entity("Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Data.Entities.DeferredSupplyProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("SupplyProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupplyProductId");

                    b.ToTable("DeferredSupplyProducts");
                });

            modelBuilder.Entity("Data.Entities.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExpenseCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("InfoMoneyId")
                        .HasColumnType("int");

                    b.Property<int?>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseCategoryId");

                    b.HasIndex("InfoMoneyId");

                    b.HasIndex("ShopId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Data.Entities.ExpenseCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExpenseCategories");
                });

            modelBuilder.Entity("Data.Entities.InfoMoney", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("MoneyOperationType")
                        .HasColumnType("int");

                    b.Property<int?>("MoneyWorkerId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<int?>("SaleId")
                        .HasColumnType("int");

                    b.Property<decimal>("Sum")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("MoneyWorkerId");

                    b.HasIndex("SaleId");

                    b.ToTable("InfoMonies");
                });

            modelBuilder.Entity("Data.Entities.InfoProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("SaleId")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("int");

                    b.Property<int?>("SupplyHistoryId")
                        .HasColumnType("int");

                    b.Property<int?>("SupplyProductId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SaleId");

                    b.HasIndex("ShopId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("SupplyHistoryId");

                    b.HasIndex("SupplyProductId");

                    b.ToTable("InfoProducts");
                });

            modelBuilder.Entity("Data.Entities.MoneyTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NextInfoMoneyId")
                        .HasColumnType("int");

                    b.Property<int>("PrevInfoMoneyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NextInfoMoneyId");

                    b.HasIndex("PrevInfoMoneyId");

                    b.ToTable("MoneyTransfers");
                });

            modelBuilder.Entity("Data.Entities.MoneyWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MoneyWorkers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("MoneyWorker");
                });

            modelBuilder.Entity("Data.Entities.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Partners");
                });

            modelBuilder.Entity("Data.Entities.PartnerProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PartnerId")
                        .HasColumnType("int");

                    b.Property<int>("ProductAmount")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PartnerId");

                    b.HasIndex("ProductId");

                    b.ToTable("PartnerProduct");
                });

            modelBuilder.Entity("Data.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookingAmount")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Reserv")
                        .HasColumnType("bit");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ShopId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Data.Entities.ProductInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AdditionalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<decimal>("FinalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("ForRealization")
                        .HasColumnType("bit");

                    b.Property<decimal>("ProcurementCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<int?>("SupplyProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SaleId");

                    b.HasIndex("SupplyProductId");

                    b.ToTable("ProductInformations");
                });

            modelBuilder.Entity("Data.Entities.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("CashSum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CashlessSum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("ForRussian")
                        .HasColumnType("bit");

                    b.Property<decimal>("Margin")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PartnerId")
                        .HasColumnType("int");

                    b.Property<bool>("Payment")
                        .HasColumnType("bit");

                    b.Property<decimal>("PrimeCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SaleType")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<decimal>("Sum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PartnerId");

                    b.HasIndex("ShopId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Data.Entities.SaleInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MoneyWorkerForCashlessIncomeId")
                        .HasColumnType("int");

                    b.Property<int?>("MoneyWorkerForExpenseId")
                        .HasColumnType("int");

                    b.Property<int?>("MoneyWorkerForIncomeId")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<int>("SaleType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MoneyWorkerForCashlessIncomeId");

                    b.HasIndex("MoneyWorkerForExpenseId");

                    b.HasIndex("MoneyWorkerForIncomeId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleInformations");
                });

            modelBuilder.Entity("Data.Entities.SaleProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Additional")
                        .HasColumnType("bit");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SaleId");

                    b.ToTable("SalesProducts");
                });

            modelBuilder.Entity("Data.Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Data.Entities.SupplyHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("SupplyHistories");
                });

            modelBuilder.Entity("Data.Entities.SupplyProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AdditionalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FinalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ProcurementCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("RealizationAmount")
                        .HasColumnType("int");

                    b.Property<int>("StockAmount")
                        .HasColumnType("int");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("int");

                    b.Property<int?>("SupplyHistoryId")
                        .HasColumnType("int");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("SupplyHistoryId");

                    b.ToTable("SupplyProducts");
                });

            modelBuilder.Entity("Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int?>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Data.Test1", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookingAmount")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CategoryTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PrimeCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<string>("ShopTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StockAmount")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Test1s");
                });

            modelBuilder.Entity("Data.ViewModels.Olds.AvailableProduct", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<int>("StockAmount")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("AvailableProducts");
                });

            modelBuilder.Entity("Data.Entities.CalculatedScore", b =>
                {
                    b.HasBaseType("Data.Entities.MoneyWorker");

                    b.HasDiscriminator().HasValue("CalculatedScore");
                });

            modelBuilder.Entity("Data.Entities.CardKeeper", b =>
                {
                    b.HasBaseType("Data.Entities.MoneyWorker");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ForManager")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("CardKeeper");
                });

            modelBuilder.Entity("Data.Entities.Shop", b =>
                {
                    b.HasBaseType("Data.Entities.MoneyWorker");

                    b.HasDiscriminator().HasValue("Shop");
                });

            modelBuilder.Entity("Data.Entities.Booking", b =>
                {
                    b.HasOne("Data.Entities.Partner", "Partner")
                        .WithMany()
                        .HasForeignKey("PartnerId");

                    b.HasOne("Data.Entities.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId");
                });

            modelBuilder.Entity("Data.Entities.BookingProduct", b =>
                {
                    b.HasOne("Data.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.DeferredSupplyProduct", b =>
                {
                    b.HasOne("Data.Entities.SupplyProduct", "SupplyProduct")
                        .WithMany()
                        .HasForeignKey("SupplyProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.Expense", b =>
                {
                    b.HasOne("Data.Entities.ExpenseCategory", "ExpenseCategory")
                        .WithMany()
                        .HasForeignKey("ExpenseCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.InfoMoney", "InfoMoney")
                        .WithMany()
                        .HasForeignKey("InfoMoneyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId");
                });

            modelBuilder.Entity("Data.Entities.InfoMoney", b =>
                {
                    b.HasOne("Data.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId");

                    b.HasOne("Data.Entities.MoneyWorker", "MoneyWorker")
                        .WithMany()
                        .HasForeignKey("MoneyWorkerId");

                    b.HasOne("Data.Entities.Sale", "Sale")
                        .WithMany()
                        .HasForeignKey("SaleId");
                });

            modelBuilder.Entity("Data.Entities.InfoProduct", b =>
                {
                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Sale", "Sale")
                        .WithMany()
                        .HasForeignKey("SaleId");

                    b.HasOne("Data.Entities.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId");

                    b.HasOne("Data.Entities.SupplyHistory", "SupplyHistory")
                        .WithMany("InfoProducts")
                        .HasForeignKey("SupplyHistoryId");

                    b.HasOne("Data.Entities.SupplyProduct", "SupplyProduct")
                        .WithMany()
                        .HasForeignKey("SupplyProductId");
                });

            modelBuilder.Entity("Data.Entities.MoneyTransfer", b =>
                {
                    b.HasOne("Data.Entities.InfoMoney", "NextInfoMoney")
                        .WithMany()
                        .HasForeignKey("NextInfoMoneyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.InfoMoney", "PrevInfoMoney")
                        .WithMany()
                        .HasForeignKey("PrevInfoMoneyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.PartnerProduct", b =>
                {
                    b.HasOne("Data.Entities.Partner", "Partner")
                        .WithMany("PartnersProducts")
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany("PartnersProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.Product", b =>
                {
                    b.HasOne("Data.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Shop", "Shop")
                        .WithMany("Products")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.ProductInformation", b =>
                {
                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("Data.Entities.Sale", "Sale")
                        .WithMany()
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.SupplyProduct", "SupplyProduct")
                        .WithMany()
                        .HasForeignKey("SupplyProductId");
                });

            modelBuilder.Entity("Data.Entities.Sale", b =>
                {
                    b.HasOne("Data.Entities.Partner", "Partner")
                        .WithMany()
                        .HasForeignKey("PartnerId");

                    b.HasOne("Data.Entities.Shop", "Shop")
                        .WithMany("Sales")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.SaleInformation", b =>
                {
                    b.HasOne("Data.Entities.MoneyWorker", "MoneyWorkerForCashlessIncome")
                        .WithMany()
                        .HasForeignKey("MoneyWorkerForCashlessIncomeId");

                    b.HasOne("Data.Entities.MoneyWorker", "MoneyWorkerForExpense")
                        .WithMany()
                        .HasForeignKey("MoneyWorkerForExpenseId");

                    b.HasOne("Data.Entities.MoneyWorker", "MoneyWorkerForIncome")
                        .WithMany()
                        .HasForeignKey("MoneyWorkerForIncomeId");

                    b.HasOne("Data.Entities.Sale", "Sale")
                        .WithMany()
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.SaleProduct", b =>
                {
                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany("SalesProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Sale", "Sale")
                        .WithMany("SalesProducts")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.SupplyProduct", b =>
                {
                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany("SupplierProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Supplier", "Supplier")
                        .WithMany("SupplyProducts")
                        .HasForeignKey("SupplierId");

                    b.HasOne("Data.Entities.SupplyHistory", "SupplyHistory")
                        .WithMany("SupplyProducts")
                        .HasForeignKey("SupplyHistoryId");
                });

            modelBuilder.Entity("Data.Entities.User", b =>
                {
                    b.HasOne("Data.Entities.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId");
                });
#pragma warning restore 612, 618
        }
    }
}
