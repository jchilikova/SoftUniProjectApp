using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Foodnetic.App.Models;
using Foodnetic.Contants;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Contact;

namespace Foodnetic.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContactService contactService;
        private readonly IMapper mapper;

        public HomeController(IContactService contactService, IMapper mapper)
        {
            this.contactService = contactService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
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

            this.contactService.SendContactMessage(contactMessage);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
