using Foodnetic.App.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class AccountController
    {
        [HttpGet]
        public IActionResult Login()
        {
            return null;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel bindingModel)
        {
            return null;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return null;
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel bindingModel)
        {
            return null;
        }
    }
}
