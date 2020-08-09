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
            var supplies1 = new Dictionary<int, int>();
            var supplies2 = new Dictionary<int, (int, int)>();
            
            supplies1.Add(8495, 19755);
            supplies1.Add(8494, 4320);
            supplies1.Add(8492, 3285);
            supplies1.Add(8490, 3285);
            
            supplies2.Add(8496, (21915, 14886));
            supplies2.Add(8493, (4320, 14942));
            //supplies2.Add(8493, (4320, 15045));
            //supplies2.Add(8491, (5490, 15196));
            supplies2.Add(8491, (5490, 15310));
            supplies2.Add(8489, (21915, 14789));
            supplies2.Add(8488, (21915, 15087));
            supplies2.Add(8487, (21915, 14897));

            supplies1.Select(x => helper1(_shopContext, x.Key, x.Value)).ToList();
            supplies2.Select(x => helper2(_shopContext, x.Key, x.Value.Item1, x.Value.Item2)).ToList();

            _shopContext.SaveChanges();
            
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