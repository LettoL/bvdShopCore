using System;
using System.IO;
using System.Linq;
using System.Text;
using Base.Services.Abstract;
using Data.Entities;
using Data.Services.Abstract;

namespace Data.Services.Concrete
{
    public class FileService : IFileService
    {
        private readonly IProductService _productService;
        private readonly IBaseObjectService<SupplyProduct> _supplyProductService;

        public FileService(IProductService productService,
            IBaseObjectService<SupplyProduct> supplyProductService)
        {
            _productService = productService;
            _supplyProductService = supplyProductService;
        }

        public void ExportProducts(int shopId, string fileName)
        {
            var products = _productService.All().Where(x => x.ShopId == shopId)
                .ToList()
                .Where(x => _supplyProductService.All()
                    .Where(s => s.ProductId == x.Id)
                    .Sum(s => s.StockAmount) > 0)
                .OrderBy(x => x.Title).Select(x => new
                {
                    Title = x.Title,
                    Code = x.Code,
                    Amount = _supplyProductService.All()
                        .Where(s => s.ProductId == x.Id)
                        .Sum(s => s.StockAmount)
                }).ToList();

            string path = "wwwroot/ExportFiles/" + fileName + ".csv";
            
            string createText = "Номер;Артикул;Название;Количество;" + Environment.NewLine;

            int number = 1;

            foreach (var product in products)
            {
                createText += number + ";";
                createText += product.Code + ";";
                createText += product.Title + ";";
                createText += product.Amount + ";" + Environment.NewLine;

                number++;
            }

            File.WriteAllText(path, createText, Encoding.UTF8);
        }
    }
}