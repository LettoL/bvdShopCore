using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
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
            var dateFirstOperation = new DateTime(2020, 11, 12);

            var a = _db.DeletedSalesInfoOld
                .Where(x => x.Sale.DeletedDate.Value.Date >= dateFirstOperation.Date
                    && x.Sale.Date >= dateFirstOperation.Date)
                .ToList();

            var b = a.Select(x => x.Sale)
                //.Select(x => x.Products.Sum(z => z.Amount * z.ProcurementCost))
                .Sum(x => x.ProcurementCost);

            var c = _shopContext.SupplyProducts
                .Sum(x => x.ProcurementCost * x.StockAmount);
            
            
            
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