using System.Linq;
using Foodnetic.App.Models.Account;
using Foodnetic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signIn;

        public AccountController(SignInManager<User> signIn)
        {
            this.signIn = signIn;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Login(LoginViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signIn.PasswordSignInAsync(bindingModel.Username,
                    bindingModel.Password, bindingModel.RememberMe,false); 

                if (result.Succeeded)
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }
                else
                {
                    this.ViewData["Error"] = ("User with that username and password does not exists!");
                }
            }

            return View(bindingModel); 
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel bindingModel)
        {
            var userExists = this.signIn.UserManager.Users.Any(x => x.UserName == bindingModel.Username);
            var emailExists = this.signIn.UserManager.Users.Any(x => x.Email == bindingModel.Email);

            if (userExists)
            {
                this.ViewData["Error"] = "User with that username already exists!";
                return this.View(bindingModel);
            }

            if (emailExists)
            {
                this.ViewData["Error"] = "User with that email already exists!";
                return this.View(bindingModel);
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = bindingModel.FirstName,
                    LastName = bindingModel.LastName,
                    UserName = bindingModel.Username,
                    Email = bindingModel.Email,
                };

                var result = this.signIn.UserManager.CreateAsync(user, bindingModel.Password).Result;

                if (result.Succeeded)
                {
                    this.signIn.SignInAsync(user, false).Wait();
                    return this.RedirectToAction("Index", "Home");
                }

                return this.View();

            }
           
            return this.View(bindingModel);
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.signIn.SignOutAsync().Wait();

            return this.RedirectToAction("Index", "Home");
        }
    }
}
