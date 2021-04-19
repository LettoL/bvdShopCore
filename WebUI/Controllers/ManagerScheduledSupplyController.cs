﻿
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Data;
using System.Text;
using WebUI.ViewModels;
using Microsoft.EntityFrameworkCore;
using PostgresData;

namespace WebUI.Controllers
{
    public class ManagerScheduledSupply : Controller
    {
        private readonly PostgresContext _postgresContext;
        private readonly ShopContext _shopContext;
        
        public ManagerScheduledSupply(PostgresContext postgresContext,
            ShopContext shopContext)
        {
            _postgresContext = postgresContext;
            _shopContext = shopContext;
        }

        [HttpGet]
        public IActionResult SupplyList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var suppliers = _shopContext.Suppliers
                .Select(x => new SupplierVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var shops = _shopContext.Shops
                .Select(x => new ShopVM()
                {
                    Title = x.Title,
                    Id = x.Id
                }).ToList();

            var moneyWorkers = _shopContext.MoneyWorkers
                .Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var payments = _postgresContext.ScheduledDeliveryPayments
                .ToList();
            
            var scheduledSupplies = _postgresContext.ScheduledDeliveries
                .Include(x => x.Products)
                .ToList()
                .Select(x => new ScheduledDeliveryVM()
                {
                    Id = x.Id,
                    Date = x.CreatedDate.ToString("dd.MM.yyyy"),
                    Payment = x.DepositedSum,
                    ProductsExpected = x.Products
                        .Where(z => z.SupplyProductId == 0 || z.SupplyProductId == null)
                        .Select(z => z.Amount)
                        .Sum(),
                    ShopsTitles = x.Products
                        .Where(z => z.ShopId > 0)
                        .Select(z => shops.FirstOrDefault(s => s.Id == z.ShopId)?.Title ?? "")
                        .Distinct()
                        .Aggregate(new StringBuilder(), (cur, next) => cur.Append(cur.Length == 0 ? "" : ", ").Append(next))
                        .ToString(),
                    SupplierId = x.SupplierId,
                    Supplier = suppliers.FirstOrDefault(z => z.Id == x.SupplierId)?.Title ?? "",
                    ProcurementCost = x.Products.Sum(z => z.ProcurementCost * z.Amount),
                    MoneyWorkersTitles = payments.Where(z => z.ScheduledDeliveryId == x.Id)
                        .Select(z => z.MoneyWorkerId)
                        .Distinct()
                        .Select(z => moneyWorkers.FirstOrDefault(m => m.Id == z)?.Title ?? "")
                        .Aggregate(new StringBuilder(), (cur, next) => cur.Append(cur.Length == 0 ? "" : ", ").Append(next))
                        .ToString()
                })
                .Where(x => x.ProductsExpected > 0)
                .OrderByDescending(x => x.Id)
                .ToList();

            return Ok(new {scheduledSupplies, suppliers});
        }
    }
}