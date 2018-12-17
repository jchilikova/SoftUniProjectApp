using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Contants;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Products;
using Foodnetic.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Foodnetic.App.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AdminsController : Controller
    {
        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AdminsController(IProductService productService, IMapper mapper, IUserService userService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.userService = userService;
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
                    ViewData["Error"] = Constants.Messages.ProductAlreadyExistsErrorMsg;
                    return this.View(bindingModel);
                }

                var product = this.mapper.Map<Product>(bindingModel);
                this.productService.Create(product);
                return RedirectToAction("CreateProduct");

            }

            return this.View(bindingModel);
        }

        public IActionResult AllProducts(int? page)
        {
            var products = this.productService.GetAll().OrderBy(x => x.ProductType).ThenBy(x => x.Name);

            var productBindingModels = new List<AllProductsViewModel>();

            var nextPage = page ?? 1;

            foreach (var product in products)
            {
                var bindingModel = this.mapper.Map<AllProductsViewModel>(product);

                productBindingModels.Add(bindingModel);
            }

            var pageViewModel = productBindingModels.ToPagedList(nextPage, 20);

            return this.View(pageViewModel);
        }

        public IActionResult AllUsers(string data, int? page)
        {
            this.ViewData[Constants.Strings.SuccessString] = data;

            var users = this.userService.GetAll().OrderBy(x => x.UserName);
            var userBindingModels = new List<AllUsersViewModel>();

            foreach (var user in users)
            {
                var bindingModel = this.mapper.Map<AllUsersViewModel>(user);

                userBindingModels.Add(bindingModel);
            }
            var nextPage = page ?? 1;

            var pageViewModel = userBindingModels.ToPagedList(nextPage, 20);

            return this.View(pageViewModel);
        }

        public IActionResult MakeUserModerator(string id)
        {
            var data = this.userService.MakeModerator(id).Result;

            return RedirectToAction("AllUsers", new {Data = data});
        }

        public IActionResult DemoteUserFromModerator(string id)
        {
            var data = this.userService.DemoteFromModerator(id).Result;

            return RedirectToAction("AllUsers", new {Data = data});
        }
    }
}