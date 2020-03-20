using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class CategoryExpenseController : Controller
    {
        private readonly IBaseObjectService<ExpenseCategory> _expenseCategoryService;

        public CategoryExpenseController(IBaseObjectService<ExpenseCategory> expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }


        public IActionResult Index()
        {
            return View(_expenseCategoryService.All());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ExpenseCategory obj)
        {
            _expenseCategoryService.Create(obj);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_expenseCategoryService.All().FirstOrDefault(x => x.Id == id));
        }

        [HttpPost]
        public IActionResult Edit(ExpenseCategory obj)
        {
            _expenseCategoryService.Update(obj);

            return RedirectToAction("Index");      
        }
    }
}