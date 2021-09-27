using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Entities.Olds;
using Microsoft.AspNetCore.Mvc;
using PostgresData;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        private readonly PostgresContext _db;
        private readonly ShopContext _shopContext;

        public TestController(PostgresContext db, ShopContext shopContext)
        {
            _db = db;
            _shopContext = shopContext;
        }

        private Func<ShopContext, int, int, int> helper1 = (shopContext, supplyId, price) =>
        {
            var supplyProduct = shopContext.SupplyProducts
                .FirstOrDefault(x => x.Id == supplyId);

            supplyProduct.FinalCost = price;
            supplyProduct.ProcurementCost = price;
            
            return supplyId;
        };

        private Func<ShopContext, int, int, int, int> helper2 = (shopContext, supplyId, price, saleId) =>
        {
            var supplyProduct = shopContext.SupplyProducts
                .FirstOrDefault(x => x.Id == supplyId);

            supplyProduct.FinalCost = price;
            supplyProduct.ProcurementCost = price;
            
            var productInformation = shopContext.ProductInformations
                .FirstOrDefault(x => x.SupplyProductId == supplyId);

            var sale = shopContext.Sales
                .FirstOrDefault(x => x.Id == saleId);

            sale.PrimeCost = price;
            sale.Margin = sale.Margin - price;

            productInformation.ProcurementCost = price;
            productInformation.FinalCost = price;
            
            return supplyId;
        };
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
           /* var suppliersInfo = new List<SupplierInfo>()
            {
                new SupplierInfo(1, 1),
                new SupplierInfo(2, 2),
                new SupplierInfo(3, 21),
                new SupplierInfo(4, 22),
                new SupplierInfo(5, 26),
                new SupplierInfo(8, 27),
                new SupplierInfo(10, 28),
                new SupplierInfo(11, 29),
                new SupplierInfo(12, 20),
                new SupplierInfo(13, 30),
                new SupplierInfo(14, 31),
                new SupplierInfo(15, 4),
                new SupplierInfo(16, 12),
                new SupplierInfo(17, 32),
                new SupplierInfo(19, 33),
                new SupplierInfo(20, 3),
                new SupplierInfo(21, 34),
                new SupplierInfo(22, 35),
                new SupplierInfo(24, 16),
                new SupplierInfo(25, 36),
                new SupplierInfo(26, 37),
                new SupplierInfo(30, 38),
                new SupplierInfo(31, 39),
                new SupplierInfo(32, 40),
                new SupplierInfo(33, 5),
                new SupplierInfo(34, 13),
                new SupplierInfo(35, 14),
                new SupplierInfo(37, 15),
                new SupplierInfo(38, 41),
                new SupplierInfo(39, 42),
                new SupplierInfo(40, 43),
                new SupplierInfo(41, 44),
                new SupplierInfo(42, 8),
                new SupplierInfo(43, 45),
                new SupplierInfo(44, 46),
                new SupplierInfo(45, 23),
                new SupplierInfo(46, 24),
                new SupplierInfo(47, 47),
                new SupplierInfo(48, 48),
                new SupplierInfo(49, 49),
                new SupplierInfo(50, 6),
                new SupplierInfo(51, 50),
                new SupplierInfo(52, 17),
                new SupplierInfo(53, 51),
                new SupplierInfo(54, 52),
                new SupplierInfo(55, 25),
                new SupplierInfo(56, 9),
                new SupplierInfo(57, 7),
                new SupplierInfo(58, 18),
                new SupplierInfo(59, 10),
                new SupplierInfo(60, 53),
                new SupplierInfo(61, 54),
                new SupplierInfo(62, 55),
                new SupplierInfo(63, 56),
                new SupplierInfo(64, 57),
                new SupplierInfo(65, 58),
                new SupplierInfo(66, 59),
                new SupplierInfo(67, 60),
                new SupplierInfo(68, 19),
                new SupplierInfo(69, 61),
                new SupplierInfo(70, 62),
                new SupplierInfo(71, 11),
                new SupplierInfo(72, 63),
                new SupplierInfo(73, 64),
                new SupplierInfo(6, 999),
                new SupplierInfo(7, 1000),
                new SupplierInfo(9, 999),
                new SupplierInfo(18, 999),
                new SupplierInfo(23, 999),
                new SupplierInfo(27, 999),
                new SupplierInfo(28, 999),
                new SupplierInfo(29, 999),
                new SupplierInfo(36, 999),
            };

            foreach (var supplierInfo in suppliersInfo.Where(x => x.Order == 999))
            {
                supplierInfo.Remove();
            }
            
            _db.SuppliersInfos.AddRange(suppliersInfo);
            _db.SaveChanges();*/
            
            /*var dateFirstOperation = new DateTime(2020, 11, 12);

            var a = _db.DeletedSalesInfoOld
                .Where(x => x.Sale.DeletedDate.Value.Date >= dateFirstOperation.Date
                    && x.Sale.Date >= dateFirstOperation.Date)
                .ToList();

            var b = a.Select(x => x.Sale)
                //.Select(x => x.Products.Sum(z => z.Amount * z.ProcurementCost))
                .Sum(x => x.ProcurementCost);

            var c = _shopContext.SupplyProducts
                .Sum(x => x.ProcurementCost * x.StockAmount);*/
            
            
            
            /*var productInf = _shopContext.ProductInformations
                .GroupBy(x => x.SaleId)
                .Select(x => new
                {
                    SaleId = x.Key,
                    Count = x.Count()
                }).ToList();

            var saleProducts = _shopContext.SalesProducts
                .GroupBy(x => x.SaleId)
                .Select(x => new
                {
                    SaleId = x.Key,
                    Count = x.Count()
                }).ToList();

            var test = new List<int>();

            foreach (var product in productInf)
            {
                if(product.Count != (saleProducts
                    .FirstOrDefault(x => x.SaleId == product.SaleId)?.Count ?? 0))
                    test.Add(product.SaleId);
            }*/
            
            
            
            
            
            
            
            
            /*var supplies1 = new Dictionary<int, int>();
            var supplies2 = new Dictionary<int, (int, int)>();
            
            supplies2.Add(7641, (4772, 15959));

            //supplies1.Select(x => helper1(_shopContext, x.Key, x.Value)).ToList();
            supplies2.Select(x => helper2(_shopContext, x.Key, x.Value.Item1, x.Value.Item2)).ToList();

            _shopContext.SaveChanges();*/
            
            
            /*var saleId = 13504;
            var productInformation = _shopContext.ProductInformations
                .FirstOrDefault(x => x.SupplyProductId == supplyProductId);

            var sale = _shopContext.Sales
                .FirstOrDefault(x => x.Id == saleId);

            sale.PrimeCost = price;
            sale.Margin = sale.Margin - price;

            productInformation.ProcurementCost = price;
            productInformation.FinalCost = price;
            /*var supplierId = 2;

            var supplier = _shopContext.Suppliers.FirstOrDefault(x => x.Id == supplierId);

            supplier.Debt = supplier.Debt + 6710 + 6710 + 4320 + 3290 - 4900; 

            _shopContext.SaveChanges();*/
            
            return Ok("lalal");
        }
    }
}