using System;
using System.Linq;
using Base.Services.Abstract;
using Data;
using Data.Entities;
using Domain.Entities.Olds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostgresData;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private IBaseObjectService<Supplier> _supplierService;
        private IBaseObjectService<SupplyProduct> _supplyProductService;
        private PostgresContext _postgresContext;
        private ShopContext _shopContext;

        public SupplierController(IBaseObjectService<Supplier> supplierService,
            IBaseObjectService<SupplyProduct> supplyProductService,
            PostgresContext postgresContext,
            ShopContext shopContext)
        {
            _supplierService = supplierService;
            _supplyProductService = supplyProductService;
            _postgresContext = postgresContext;
            _shopContext = shopContext;
        }

        public IActionResult Index()
        {
            var suppliersInfoInit = _postgresContext.SupplierInfoInits.ToList();

           /* var a = _shopContext.Products
                .Where(x => x.ShopId == 33)
                .Select(x => x.Id)
                .ToList();*/

           /*var test = _postgresContext.ProductOperations
               .Where(x => x.SupplierId == 57)
               .ToList();
           var test1 = suppliersInfoInit.Where(x => x.SupplierId == 57).ToList();

           var test2 = _supplyProductService.All()
               .Where(p => p.SupplierId == 57)
               .Sum(p => p.StockAmount * p.ProcurementCost);*/
            
            var operations = _postgresContext.ProductOperations
               // .Where(x => a.Contains(x.ProductId))
                .ToList()
                .GroupBy(x => x.SupplierId)
                .Select(x => new
                {
                    SupplierId = x.Key,
                    Debt = x.Where(z => z.ForRealization && z.Amount < 0)
                        .Sum(z => z.Cost * z.Amount * -1),
                    OnStockForRealization = x.Where(z => z.ForRealization 
                                                         && z.StorageType == StorageType.Shop)
                        .Sum(z => z.Amount * z.Cost),
                    OnStock = x.Where(z => z.StorageType == StorageType.Shop)
                        .Sum(z => z.Amount * z.Cost)
                }).ToList();

            var repayments = _postgresContext.SupplierPayments.ToList()
                .GroupBy(x => x.SupplierId)
                .Select(x => new
                {
                    SupplierId = x.Key,
                    RepaymentsSum = x.Sum(z => z.Sum)
                }).ToList();

            /*_postgresContext.SupplierInfoInits.RemoveRange(_postgresContext.SupplierInfoInits.ToList());
            _postgresContext.SaveChanges();*/
            
            /*var result = _supplierService.All().Select(x => new SupplierVM()
            {
                Id = x.Id,
                Title = x.Title,
                Phone = x.Phone,
                Email = x.Email,
                Debt = x.Debt,
                CostRealizationProductOnStock = _supplyProductService.All()
                    .Where(p => p.SupplierId == x.Id)
                    .Sum(p => p.RealizationAmount * p.ProcurementCost),
                CostProductOnStock = _supplyProductService.All()
                    .Where(p => p.SupplierId == x.Id)
                    .Sum(p => p.StockAmount * p.ProcurementCost)
            }).ToList();

            foreach (var supplier in result)
            {
                _postgresContext.SupplierInfoInits.Add(new SupplierInfoInit(
                    supplier.Id,
                    supplier.Debt,
                    supplier.CostRealizationProductOnStock,
                    supplier.CostProductOnStock));
            }
            _postgresContext.SaveChanges();*/

            var result = _supplierService.All().Select(x => new SupplierVM()
            {
                Id = x.Id,
                Title = x.Title,
                Phone = x.Phone,
                Email = x.Email,
                Debt = 0,
                CostRealizationProductOnStock = 0,
                CostProductOnStock = 0
            }).ToList();

            foreach (var supplierVm in result)
            {
                var supplierInfoInit = suppliersInfoInit
                    .FirstOrDefault(x => x.SupplierId == supplierVm.Id);
                if (supplierInfoInit != null)
                {
                    supplierVm.Debt += supplierInfoInit.Debt;
                    supplierVm.CostProductOnStock += supplierInfoInit.PriceProducts;
                    supplierVm.CostRealizationProductOnStock += supplierInfoInit.PriceProductsForRealization;
                }

                var operation = operations
                    .FirstOrDefault(x => x.SupplierId == supplierVm.Id);
                if (operation != null)
                {
                    supplierVm.Debt += operation.Debt;
                    supplierVm.CostProductOnStock += operation.OnStock;
                    supplierVm.CostRealizationProductOnStock += operation.OnStockForRealization;
                }

                var repayment = repayments
                    .FirstOrDefault(x => x.SupplierId == supplierVm.Id);
                if (repayment != null)
                {
                    supplierVm.Debt -= repayment.RepaymentsSum;
                }
            }

            ViewBag.Debt = Math.Round(result.Sum(x => x.Debt), 2);
            ViewBag.RealizationCost = Math.Round(result.Sum(x => x.CostRealizationProductOnStock), 2);
            ViewBag.CostProductOnStock = Math.Round(result.Sum(x => x.CostProductOnStock), 2);
            
            return View(result);
        }

        [HttpPost]
        public IActionResult SupplierInfoByDate(string date)
        {
            var filterDate = DateTime.Now.AddHours(3);

            if (date != null)
            {
                var buf = date.Split('.');
                filterDate = new DateTime(
                    Convert.ToInt32(buf[2]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[0]));
            }
            
            var suppliersInfoInit = _postgresContext.SupplierInfoInits.ToList();

            var operations = _postgresContext.ProductOperations
                .Where(x => x.DateTime.Date <= filterDate.Date)
                .ToList()
                .GroupBy(x => x.SupplierId)
                .Select(x => new
                {
                    SupplierId = x.Key,
                    Debt = x.Where(z => z.ForRealization && z.Amount < 0)
                        .Sum(z => z.Cost * z.Amount * -1),
                    OnStockForRealization = x.Where(z => z.ForRealization)
                        .Sum(z => z.Amount * z.Cost),
                    OnStock = x.Sum(z => z.Amount * z.Cost)
                }).ToList();

            var repayments = _postgresContext.SupplierPayments
                .Where(x => x.DateTime.Date <= filterDate.Date)
                .ToList()
                .GroupBy(x => x.SupplierId)
                .Select(x => new
                {
                    SupplierId = x.Key,
                    RepaymentsSum = x.Sum(z => z.Sum)
                }).ToList();

            var result = _supplierService.All().Select(x => new SupplierVM()
            {
                Id = x.Id,
                Title = x.Title,
                Phone = x.Phone,
                Email = x.Email,
                Debt = 0,
                CostRealizationProductOnStock = 0,
                CostProductOnStock = 0
            }).ToList();

            foreach (var supplierVm in result)
            {
                var supplierInfoInit = suppliersInfoInit
                    .FirstOrDefault(x => x.SupplierId == supplierVm.Id);
                if (supplierInfoInit != null)
                {
                    supplierVm.Debt += supplierInfoInit.Debt;
                    supplierVm.CostProductOnStock += supplierInfoInit.PriceProducts;
                    supplierVm.CostRealizationProductOnStock += supplierInfoInit.PriceProductsForRealization;
                }

                var operation = operations
                    .FirstOrDefault(x => x.SupplierId == supplierVm.Id);
                if (operation != null)
                {
                    supplierVm.Debt += operation.Debt;
                    supplierVm.CostProductOnStock += operation.OnStock;
                    supplierVm.CostRealizationProductOnStock += operation.OnStockForRealization;
                }

                var repayment = repayments
                    .FirstOrDefault(x => x.SupplierId == supplierVm.Id);
                if (repayment != null)
                {
                    supplierVm.Debt -= repayment.RepaymentsSum;
                }
            }


            ViewBag.Debt = Math.Round(result.Sum(x => x.Debt), 2);
            ViewBag.RealizationCost = Math.Round(result.Sum(x => x.CostRealizationProductOnStock), 2);
            ViewBag.CostProductOnStock = Math.Round(result.Sum(x => x.CostProductOnStock), 2);
            
            return PartialView(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Supplier obj)
        {
            _supplierService.Create(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Supplier supplier = _supplierService.All().First(u => u.Id == id);
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(int id, string title, string email, string phone)
        {
            _supplierService.Update(new Supplier()
            {
                Id = id,
                Title = title,
                Email = email,
                Phone = phone
            });

            return RedirectToAction("Index", "Supplier");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _supplierService.Delete(id);

            return RedirectToAction("Index", "Supplier");
        }
    }
}
