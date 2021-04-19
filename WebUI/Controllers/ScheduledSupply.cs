using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using Data.Entities;
using Data.Enums;
using Domain.Entities;
using Domain.Entities.Olds;
using Handlers.CommandHandlers;
using Handlers.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresData;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class ScheduledSupply : Controller
    {
        private readonly PostgresContext _postgresContext;
        private readonly ShopContext _shopContext;
        
        public ScheduledSupply(PostgresContext postgresContext,
            ShopContext shopContext)
        {
            _postgresContext = postgresContext;
            _shopContext = shopContext;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var products = _shopContext.Products
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var suppliers = _shopContext.Suppliers
                .Select(x => new SupplierVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var categories = _shopContext.Categories
                .Select(x => new CategoryVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var shops = _shopContext.Shops
                .Select(x => new ShopVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            ViewBag.Products = products;
            ViewBag.Suppliers = suppliers;
            ViewBag.Categories = categories;
            ViewBag.Shops = shops;
            
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateScheduledDeliveryVM scheduledDelivery)
        {
            try
            {
                var products = new List<ScheduledProductDelivery>();

                foreach (var product in scheduledDelivery.Products)
                {
                    ScheduledProductDelivery productDelivery;

                    if (product.ProductId != 0)
                    {
                        productDelivery = new ScheduledProductDelivery(product.ProductId, product.Amount,
                            scheduledDelivery.SupplierId, product.ProcurementCost);
                    }
                    else
                    {
                        var existingProduct = _shopContext.Products
                            .FirstOrDefault(x => x.Title == product.Title);

                        if (existingProduct == null)
                        {
                            var newProduct = _shopContext.Products.Add(new Product()
                            {
                                Title = product.Title,
                                Code = product.Code,
                                ShopId = 1,
                                CategoryId = scheduledDelivery.CategoryId,
                                Cost = product.ProcurementCost
                            });
                            _shopContext.SaveChanges();

                            productDelivery = new ScheduledProductDelivery(newProduct.Entity.Id, product.Amount,
                                scheduledDelivery.SupplierId, product.ProcurementCost);
                        }
                        else
                        {
                            productDelivery = new ScheduledProductDelivery(existingProduct.Id, product.Amount,
                                scheduledDelivery.SupplierId, product.ProcurementCost);
                        }
                    }

                    if (scheduledDelivery.ShopId > 0)
                        productDelivery.ShopId = scheduledDelivery.ShopId;

                    products.Add(productDelivery);
                }

                var createdSchedulerDelivery = _postgresContext.ScheduledDeliveries
                    .Add(new ScheduledDelivery(
                        scheduledDelivery.SupplierId,
                        scheduledDelivery.DepositedSum,
                        products));

                var supplier = _shopContext.Suppliers.FirstOrDefault(x => x.Id == scheduledDelivery.SupplierId);

                var infoMoney = _shopContext.InfoMonies.Add(new InfoMoney()
                {
                    Sum = -scheduledDelivery.DepositedSum,
                    PaymentType = PaymentType.Cashless,
                    MoneyWorkerId = scheduledDelivery.MoneyWorkerId,
                    MoneyOperationType = MoneyOperationType.Expense,
                    Date = DateTime.Now.AddHours(3),
                    Comment = supplier?.Title ?? ""
                }).Entity;

                var expense = _shopContext.Expenses.Add(new Expense()
                {
                    InfoMoney = infoMoney,
                    ExpenseCategoryId = 4
                });

                _shopContext.SaveChanges();

                // _postgresContext.SupplierPayments.Add(
                //     new SupplierPayment(scheduledDelivery.DepositedSum, scheduledDelivery.SupplierId, DateTime.Now.AddHours(3)));
                _postgresContext.ExpensesOld.Add(
                    new ExpenseOld(
                        expense.Entity.Id,
                        scheduledDelivery.ShopId > 0 ? scheduledDelivery.ShopId : 1));

                _postgresContext.SaveChanges();

                _postgresContext.ScheduledDeliveryPayments.Add(new ScheduledDeliveryPayment()
                {
                    InfoMoneyId = infoMoney.Id,
                    ScheduledDeliveryId = createdSchedulerDelivery.Entity.Id,
                    MoneyWorkerId = scheduledDelivery.MoneyWorkerId
                });

                _postgresContext.SaveChanges();

                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            ViewBag.Id = id;
            
            return View();
        }

        [HttpPost]
        public IActionResult Detail([FromBody]EditScheduledDelivery command)
        {
            ScheduledDeliveryEditHandler.Execute(command, _postgresContext);
            //ScheduledDeliveryConfirmHandler.Execute(command.DeliveryId, _postgresContext, _shopContext);
            
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Payment(int id)
        {
            var scheduledDelivery = _postgresContext.ScheduledDeliveries
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == id);

            if (scheduledDelivery == null)
                throw new Exception("Поставка не найдена");

            var rest = scheduledDelivery.Products.Sum(x => x.ProcurementCost)
                - scheduledDelivery.DepositedSum;

            var shops = _shopContext.Shops
                .Select(x => new ShopVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();
            
            ViewBag.Id = id;
            ViewBag.Rest = rest;
            ViewBag.Shops = shops;
            
            return View();
        }
        
        [HttpPost]
        public IActionResult Payment(ScheduledSupplyPaymentCreateVM payment)
        {
            var supply = _postgresContext.ScheduledDeliveries
                .FirstOrDefault(x => x.Id == payment.SupplyId);

            if (supply == null)
                throw new Exception("Поставка не найден");

            supply.DepositedSum += payment.Sum;

            var supplier = _shopContext.Suppliers.FirstOrDefault(x => x.Id == supply.SupplierId);
            
            var infoMoney = _shopContext.InfoMonies.Add(new InfoMoney()
            {
                Sum = -payment.Sum,
                PaymentType = PaymentType.Cashless,
                MoneyWorkerId = payment.MoneyWorkerId,
                MoneyOperationType = MoneyOperationType.Expense,
                Date = DateTime.Now.AddHours(3),
                Comment = supplier?.Title ?? ""
            }).Entity;

            var expense = _shopContext.Expenses.Add(new Expense()
            {
                InfoMoney = infoMoney,
                ExpenseCategoryId = 4
            });
            
            _shopContext.SaveChanges();

            //_postgresContext.SupplierPayments.Add(
            //    new SupplierPayment(payment.Sum, supply.SupplierId, DateTime.Now.AddHours(3)));
            _postgresContext.ExpensesOld.Add(
                new ExpenseOld(expense.Entity.Id, payment.ShopId));

            _postgresContext.ScheduledDeliveryPayments.Add(new ScheduledDeliveryPayment()
            {
                InfoMoneyId = infoMoney.Id,
                MoneyWorkerId = payment.MoneyWorkerId,
                ScheduledDeliveryId = supply.Id
            });

            _postgresContext.SaveChanges();
            
            return RedirectToAction("List");        
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var scheduledDelivery = _postgresContext.ScheduledDeliveries
                .FirstOrDefault(x => x.Id == id);

            var scheduledDeliveryProducts = _postgresContext.ScheduledProductDeliveries
                .Where(x => x.ScheduledDeliveryId == id)
                .ToList();

            var scheduledDeliveryPayments = _postgresContext.ScheduledDeliveryPayments
                .Where(x => x.ScheduledDeliveryId == id)
                .ToList();

            var infoMoneys = _shopContext.InfoMonies
                .Where(x => scheduledDeliveryPayments
                    .Select(z => z.InfoMoneyId)
                    .Contains(x.Id))
                .ToList();

            var expenses = _shopContext.Expenses
                .Where(x => infoMoneys
                    .Select(z => z.Id)
                    .Contains(x.InfoMoneyId))
                .ToList();

            var expensesOld = _postgresContext.ExpensesOld
                .Where(x => expenses
                    .Select(z => z.Id)
                    .Contains(x.ExpenseId))
                .ToList();

            var supplyProducts = _shopContext.SupplyProducts
                .Where(x => scheduledDeliveryProducts
                    .Where(z => z.SupplyProductId > 0)
                    .Select(z => z.SupplyProductId)
                    .Contains(x.Id))
                .ToList();

            var supplyHistories = _shopContext.SupplyHistories
                .Where(x => supplyProducts
                    .Where(z => z.SupplyHistoryId > 0)
                    .Select(z => z.SupplyHistoryId)
                    .Contains(x.Id))
                .ToList();

            var infoProducts = _shopContext.InfoProducts
                .Where(x => x.SupplyHistoryId > 0)
                .Where(x => supplyHistories
                    .Select(z => z.Id)
                    .Contains((int)x.SupplyHistoryId))
                .ToList();
            
            
            _shopContext.InfoProducts.RemoveRange(infoProducts);
            _shopContext.SupplyHistories.RemoveRange(supplyHistories);
            _shopContext.SupplyProducts.RemoveRange(supplyProducts);
            _shopContext.Expenses.RemoveRange(expenses);
            _shopContext.InfoMonies.RemoveRange(infoMoneys);
            
            _postgresContext.ExpensesOld.RemoveRange(expensesOld);
            _postgresContext.ScheduledDeliveryPayments.RemoveRange(scheduledDeliveryPayments);
            _postgresContext.ScheduledProductDeliveries.RemoveRange(scheduledDeliveryProducts);
            _postgresContext.ScheduledDeliveries.Remove(scheduledDelivery);

            _shopContext.SaveChanges();
            _postgresContext.SaveChanges();
            
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult ChangeShop([FromBody]int scheduledSupplyId, [FromBody]int shopId)
        {
            var scheduledSupply = _postgresContext.ScheduledProductDeliveries
                .FirstOrDefault(x => x.Id == scheduledSupplyId);

            if (scheduledSupply == null)
                throw new Exception("Поставка не найдена");

            scheduledSupply.ShopId = shopId;

            _postgresContext.ScheduledProductDeliveries.Update(scheduledSupply);
            _postgresContext.SaveChanges();
            
            return Ok();
        }

        [HttpGet]
        public IActionResult Products(int id)
        {
            var shops = _shopContext.Shops
                .Select(x => new ShopVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var productsTitles = _shopContext.Products
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var products = _postgresContext.ScheduledProductDeliveries.ToList()
                .Where(x => x.ScheduledDeliveryId == id)
                .Select(x => new ScheduledProductDeliveryVM()
                {
                    Id = x.Id,
                    Title = productsTitles.FirstOrDefault(z => z.Id == x.ProductId)?.Title ?? "",
                    Amount = x.Amount,
                    ShopId = x.ShopId,
                    Shop = shops.FirstOrDefault(z => z.Id == x.ShopId)?.Title ?? "Магазин не выбран",
                    ProcurementCost = x.ProcurementCost,
                    ProductId = x.ProductId,
                    SupplyProductId = x.SupplyProductId ?? 0
                })
                .ToList();
            
            return Ok(new {shops, products});
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

        [HttpGet]
        public IActionResult GetProductsList()
        {
            var productsTitles = _shopContext.Products
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var shops = _shopContext.Shops
                .Select(x => new ShopVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var suppliers = _shopContext.Suppliers
                .Select(x => new SupplierVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var scheduledProductsDeliveries = _postgresContext.ScheduledProductDeliveries
                .Where(x => x.DeliveryType != ScheduledProductDeliveryType.Delivered)
                .ToList()
                .Select(x => new ScheduledProductDeliveryVM()
                {
                    Id = x.Id,
                    Title = productsTitles.FirstOrDefault(z => z.Id == x.ProductId).Title,
                    Amount = x.Amount,
                    ProcurementCost = x.ProcurementCost,
                    Shop = x.ShopId == 0
                        ? "Магазин не выбран"
                        : shops.FirstOrDefault(z => z.Id == x.ShopId).Title,
                    Supplier = suppliers.FirstOrDefault(z => z.Id == x.SupplierId).Title,
                    ShopId = x.ShopId,
                    SupplierId = x.SupplierId
                }).ToList();
            
            return Ok(scheduledProductsDeliveries);
        }

        [HttpGet]
        public IActionResult GetFilters()
        {
            var shops = _shopContext.Shops
                .Select(x => new ShopVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            var suppliers = _shopContext.Suppliers
                .Select(x => new SupplierVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();
            
            return Ok(new
            {
                suppliers = suppliers,
                shops = shops
            });
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _shopContext.Products
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Category = x.Category
                }).ToList();

            var categories = _shopContext.Categories
                .Select(x => new CategoryVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();

            return Ok(new {products, categories});
        }
    }
}