using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels;
using Foodnetic.ViewModels.Contact;

namespace Foodnetic.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContactService contactService;
        private readonly IMapper mapper;
        private readonly IRecipeService recipeService;

        public HomeController(IContactService contactService, IMapper mapper, IRecipeService recipeService)
        {
            this.contactService = contactService;
            this.mapper = mapper;
            this.recipeService = recipeService;
        }

        public IActionResult Index()
        {
            this.recipeService.CancelRecipe(this.User.Identity.Name);
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactUsViewModel bindingModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(bindingModel);
            }

            var contactMessage = this.mapper.Map<ContactMessage>(bindingModel);

            this.contactService.CreateContactMessage(contactMessage);

            return RedirectToAction("Index");
        }

    }
}
