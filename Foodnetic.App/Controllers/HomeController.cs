using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Foodnetic.App.Models;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
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
            this.recipeService.CancelRecipe();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
