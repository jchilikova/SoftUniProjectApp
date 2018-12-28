using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Contants;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Contact;
using Foodnetic.ViewModels.Products;
using Foodnetic.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IContactService contactService;

        public AdminsController(IProductService productService, IMapper mapper, IUserService userService, IContactService contactService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.userService = userService;
            this.contactService = contactService;
        }

        [HttpGet]
        [Authorize(Roles = Constants.Strings.AdministratorRole)]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constants.Strings.AdministratorRole)]
        public IActionResult CreateProduct(CreateProductViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                if (this.productService.CheckIfProductExists(bindingModel.Name))
                {
                    ViewData[Constants.Strings.ErrorString] = Constants.Messages.ProductAlreadyExistsErrorMsg;
                    return this.View(bindingModel);
                }

                var product = this.mapper.Map<Product>(bindingModel);
                this.productService.Create(product);
                return RedirectToAction("CreateProduct");

            }

            return this.View(bindingModel);
        }

        [Authorize(Roles = Constants.Strings.AdministratorRole)]
        public IActionResult AllProducts(int? page)
        {
            var products = this.productService.GetAll();

            if (products == null)
            {
                return this.View();
            }

            var orderedEnumerable = products.OrderBy(x => x.ProductType).ThenBy(x => x.Name);

            var nextPage = page ?? 1;

            var productBindingModels = this.MapAllProducts(orderedEnumerable);
           
            var pageViewModel = productBindingModels.ToPagedList(nextPage, 20);

            return this.View(pageViewModel);
        }

        private IEnumerable<AllProductsViewModel> MapAllProducts(IEnumerable<Product> products)
        {
            var productBindingModels = new List<AllProductsViewModel>();

            foreach (var product in products)
            {
                var bindingModel = this.mapper.Map<AllProductsViewModel>(product);

                productBindingModels.Add(bindingModel);
            }

            return productBindingModels;
        }

        [Authorize(Roles = Constants.Strings.AdministratorRole)]
        public IActionResult AllUsers(string data, int? page)
        {
            this.ViewData[Constants.Strings.SuccessString] = data;

            var users = this.userService.GetAll().OrderBy(x => x.UserName);

            var userBindingModels = this.MapAllUsers(users);
          
            var nextPage = page ?? 1;

            var pageViewModel = userBindingModels.ToPagedList(nextPage, 20);

            return this.View(pageViewModel);
        }

        private IEnumerable<AllUsersViewModel> MapAllUsers(IEnumerable<FoodneticUser> users)
        {
            var userBindingModels =  new List<AllUsersViewModel>();

            foreach (var user in users)
            {
                var bindingModel = this.mapper.Map<AllUsersViewModel>(user);

                userBindingModels.Add(bindingModel);
            }

            return userBindingModels;
        }

        [Authorize(Roles = Constants.Strings.AdministratorRole)]
        public IActionResult MakeUserModerator(string id)
        {
            var data = this.userService.MakeModerator(id).Result;

            return RedirectToAction("AllUsers", new {Data = data});
        }

        [Authorize(Roles = Constants.Strings.AdministratorRole)]
        public IActionResult DemoteUserFromModerator(string id)
        {
            var data = this.userService.DemoteFromModerator(id).Result;

            return RedirectToAction("AllUsers", new {Data = data});
        }

        [Authorize(Roles = Constants.Strings.AdministratorRole)]
        public IActionResult AllContactUsMessages(int? page)
        {
            var contactMessages = contactService.GetAll();

            if (contactMessages == null)
            {
                return this.View();
            }

            var bindingModels = this.MapAllMessages(contactMessages);

            var nextPage = page ?? 1;

            var pageViewModel = bindingModels.ToPagedList(nextPage, 20);


            return this.View(pageViewModel);
        }

        private IEnumerable<AllContactUsMessagesViewModel> MapAllMessages(IEnumerable<ContactMessage> messages)
        {
            var contactMessagesBindingModels =  new List<AllContactUsMessagesViewModel>();

            foreach (var message in messages)
            {
                var bindingModel = this.mapper.Map<AllContactUsMessagesViewModel>(message);

                contactMessagesBindingModels.Add(bindingModel);
            }

            return contactMessagesBindingModels;
        }
    }
}