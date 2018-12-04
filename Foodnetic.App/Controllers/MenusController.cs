using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class MenusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}