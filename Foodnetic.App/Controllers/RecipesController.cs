using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class RecipesController : Controller
    {
        public IActionResult All()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        //[HttpPost]
        //public IActionResult Create()
        //{
        //    return this.View();
        //}
    }
}
