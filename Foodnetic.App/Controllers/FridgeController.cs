using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Grocery;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class FridgeController : Controller
    {
        private readonly IFridgeService fridgeService;
        private readonly IProductService productService;

        public FridgeController(IFridgeService fridgeService, IProductService productService)
        {
            this.fridgeService = fridgeService;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            IEnumerable<Grocery> groceries = this.fridgeService.GetAll(this.User.Identity.Name);
            return this.View(groceries);
        }

        [HttpGet]
        public IActionResult AddGrocery(string searchString)
        {
            IEnumerable<Product> products = this.productService.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => (p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                                                || p.ProductType.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)) 
                                               && p.GetType() == typeof(Product));
                var bindingModel = new CreateGroceryViewModel
                {
                    Products = products
                };
                return this.View(bindingModel);
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult AddGrocery(CreateGroceryViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                this.fridgeService.CreateGrocery(bindingModel, this.User.Identity.Name);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}