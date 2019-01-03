using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Constants;
using Foodnetic.Infrastructure;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Groceries;
using Foodnetic.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Foodnetic.App.Controllers
{
    public class FridgeController : Controller
    {
        private readonly IFridgeService fridgeService;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public FridgeController(IFridgeService fridgeService, IProductService productService, IMapper mapper)
        {
            this.fridgeService = fridgeService;
            this.productService = productService;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Index(int? page)
        {
            var groceries = this.fridgeService.GetAllGroceries(this.User.Identity.Name);

            if (groceries == null)
            {
                return this.View();
            }

            var orderedGroceries = groceries.OrderBy(x => x.ExpirationDate);

            var groceriesViewModels = this.MapAllGroceries(orderedGroceries);

            var nextPage = page ?? 1;

            var pageViewModel = groceriesViewModels.ToPagedList(nextPage, 10);

            return this.View(pageViewModel);
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult AddGrocery(string searchString, string data)
        {
            this.ViewData[GlobalConstants.ErrorString] = data;

            var products = this.productService.GetAll();

            if (string.IsNullOrEmpty(searchString)) return this.View();

            var bindingModel = this.SearchForGrocery(products, searchString);

            return this.View(bindingModel);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult AddGrocery(CreateGroceryViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                this.fridgeService.CreateGrocery(bindingModel, this.User.Identity.Name);
                return RedirectToAction("Index");
            }

            var data = ConstantMessages.InvalidDataMsg;
            return RedirectToAction("AddGrocery", new {Data = data});
        }

        private IEnumerable<GroceryViewModel> MapAllGroceries(IEnumerable<Grocery> groceries)
        {
            var groceriesViewModels = new List<GroceryViewModel>();

            foreach (var grocery in groceries)
            {
                var bindingModel = this.mapper.Map<GroceryViewModel>(grocery);
                groceriesViewModels.Add(bindingModel);
            }

            return groceriesViewModels;
        }

        private CreateGroceryViewModel SearchForGrocery(IEnumerable<Product> products, string searchString)
        {
            products = products.Where(p => (p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                                            || p.ProductType.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)) 
                                           && p.GetType() == typeof(Product));

            var productsBindingModels = products.Select(product => this.mapper.Map<ProductViewModel>(product)).ToList();

            var bindingModel = new CreateGroceryViewModel
            {
                Products = productsBindingModels
            };

            return bindingModel;
        }

    }
}