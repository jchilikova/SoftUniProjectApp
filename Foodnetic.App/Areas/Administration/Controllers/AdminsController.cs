using Foodnetic.Contants;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AdminsController : Controller
    {
        private readonly IProductService productService;

        public AdminsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                if (this.productService.CheckIfProductExists(bindingModel.Name))
                {
                    ViewData["Error"] = Constants.Messages.ProductAlreadyExists;
                    return this.View(bindingModel);
                }

                this.productService.Create(bindingModel);
                return RedirectToAction("CreateProduct");

            }

            return this.View(bindingModel);
        }
    }
}