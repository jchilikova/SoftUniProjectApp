using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AdminsController : Controller
    {
        public IActionResult CreateProduct()
        {
            return View();
        }
    }
}