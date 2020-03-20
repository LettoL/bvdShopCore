using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private IBaseObjectService<Supplier> _supplierService;
        private IBaseObjectService<SupplyProduct> _supplyProductService;

        public SupplierController(IBaseObjectService<Supplier> supplierService,
            IBaseObjectService<SupplyProduct> supplyProductService)
        {
            _supplierService = supplierService;
            _supplyProductService = supplyProductService;
        }

        public IActionResult Index()
        {
            return View(_supplierService.All().Select(x => new SupplierVM()
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
            }));
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
