using System.Linq;
using System.Threading.Tasks;
using Foodnetic.Contants;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;

        public AccountController(SignInManager<User> signInManager, IUserService userService)
        {
            this.signInManager = signInManager;
            this.userService = userService;
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
                var result = await signInManager.PasswordSignInAsync(bindingModel.Username,
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
        public async System.Threading.Tasks.Task<IActionResult> Register(RegisterViewModel bindingModel)
        {
            var userExists = this.signInManager.UserManager.Users.Any(x => x.UserName == bindingModel.Username);
            var emailExists = this.signInManager.UserManager.Users.Any(x => x.Email == bindingModel.Email);

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

                var result = this.signInManager.UserManager.CreateAsync(user, bindingModel.Password).Result;

                if (result.Succeeded)
                {
                    await this.signInManager.UserManager.AddToRoleAsync(user, Constants.Strings.UserRole);
                    this.signInManager.SignInAsync(user, false).Wait();
                    return this.RedirectToAction("Index", "Home");
                }

                return this.View();

            }
           
            return this.View(bindingModel);
        }

        public async Task<IActionResult> ExternalLogin()
        {
            var info = await this.signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var result = await this.userService.ExternalLoginUser(info);

            if (result)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = "/Account/ExternalLogin";
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync().Wait();

            return this.RedirectToAction("Index", "Home");
        }
    }
}
