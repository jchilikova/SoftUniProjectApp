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
        private readonly IUserService userService;
        private readonly SignInManager<User> signInManager;

        public AccountController(IUserService userService, SignInManager<User> signInManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel bindingModel)
        {
            if (!ModelState.IsValid) return View(bindingModel);

            var result = this.userService.SignInUser(bindingModel);

            if (await result)
            {
                return RedirectToAction("Index", "Home");
            }

            this.ViewData[Constants.Strings.ErrorString] = Constants.Messages.UserAlreadyExistsErrorMsg;

            return View(bindingModel); 
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel bindingModel)
        {
            var userExists = this.userService.CheckIfUserExists(bindingModel.Username);

            if (userExists)
            {
                this.ViewData[Constants.Strings.ErrorString] = Constants.Messages.UsernameAlreadyExistsErrorMsg;
                return this.View(bindingModel);
            }

            var emailExists = this.userService.CheckIfEmailExists(bindingModel.Email);

            if (emailExists)
            {
                this.ViewData[Constants.Strings.ErrorString] = Constants.Messages.EmailAlreadyExistsErrorMsg;
                return this.View(bindingModel);
            }

            if (ModelState.IsValid)
            {
                var user = this.userService.CreateUser(bindingModel);

                if (user == null) return this.View();

                await this.userService.AddToRole(user);
                this.signInManager.SignInAsync(user, false).Wait();
                return this.RedirectToAction("Index", "Home");

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
            const string redirectUrl = Constants.Strings.ExternalLoginRedirect;
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
