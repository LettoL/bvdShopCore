using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private IBaseObjectService<Category> _categoryService { get; set; }

        public CategoryController(IBaseObjectService<Category> categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View(_categoryService.All().Select(x => new CategoryVM()
            {
                Id = x.Id,
                Title = x.Title
            }));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_categoryService.All().Select(x => new CategoryVM()
            {
                Id = x.Id,
                Title = x.Title
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            _categoryService.Create(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(_categoryService.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            _categoryService.Update(obj);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);

            return RedirectToAction("Index", "Category");
        }
    }
}
