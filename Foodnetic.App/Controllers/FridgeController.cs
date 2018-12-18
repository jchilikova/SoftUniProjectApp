﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
        private readonly IMapper mapper;

        public FridgeController(IFridgeService fridgeService, IProductService productService, IMapper mapper)
        {
            this.fridgeService = fridgeService;
            this.productService = productService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var groceries = this.fridgeService.GetAll(this.User.Identity.Name);

            if (groceries == null)
            {
                return this.View();
            }

            var groceriesViewModels = this.MapAllGroceries(groceries);

            return this.View(groceriesViewModels);
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

        [HttpGet]
        public IActionResult AddGrocery(string searchString)
        {
            var products = this.productService.GetAll();

            if (string.IsNullOrEmpty(searchString)) return this.View();

            var bindingModel = this.SearchForGrocery(products, searchString);

            return this.View(bindingModel);

        }

        private CreateGroceryViewModel SearchForGrocery(IEnumerable<Product> products, string searchString)
        {
            products = products.Where(p => (p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                                            || p.ProductType.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)) 
                                           && p.GetType() == typeof(Product));

            var bindingModel = new CreateGroceryViewModel
            {
                Products = products
            };

            return bindingModel;
        }

        [HttpPost]
        public IActionResult AddGrocery(CreateGroceryViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                this.fridgeService.CreateGrocery(bindingModel, this.User.Identity.Name);
                return RedirectToAction("Index");
            }

            return RedirectToAction("AddGrocery");
        }
    }
}