using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Foodnetic.Contants;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly FoodneticDbContext dbContext;

        public UserService(SignInManager<User> signInManager, IMapper mapper, FoodneticDbContext dbContext, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task AddToRole(User user)
        {
            await this.signInManager.UserManager.AddToRoleAsync(user, Constants.Strings.UserRole);
        }

        public bool CheckIfEmailExists(string email)
        {
            var emailExists = this.signInManager.UserManager.Users.Any(x => x.Email == email);

            return emailExists;

        }

        public bool CheckIfUserExists(string username)
        {
            var userExists = this.signInManager.UserManager.Users.Any(x => x.UserName == username);

            return userExists;
        }

        public User CreateUser(RegisterViewModel bindingModel)
        {
            var user = this.mapper.Map<User>(bindingModel);

            var result = this.signInManager.UserManager.CreateAsync(user, bindingModel.Password).Result;

            return result.Succeeded ? user : null;
        }

        public async Task<bool> ExternalLoginUser(ExternalLoginInfo info)
        {

            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = await this.signInManager.UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                };

                var result = await this.signInManager.UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    var addExternalLogin = await this.signInManager.UserManager.AddLoginAsync(user, info);

                    if (addExternalLogin.Succeeded)
                    {
                        var roleResult = this.signInManager.UserManager.AddToRoleAsync(user, Constants.Strings.UserRole).Result;

                        if (roleResult.Errors.Any())
                        {
                            return false;
                        }

                        await this.signInManager.SignInAsync(user, isPersistent: false);

                        return true;
                    }
                }

                return false;
            }
            else
            {
                var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                    isPersistent: false, bypassTwoFactor: true);

                if (result.Succeeded)
                {
                    return true;
                }

                return false;
            }

        }

        public IEnumerable<User> GetAll()
        {
            return this.signInManager.UserManager.Users.Where(x => x.UserName != Constants.Strings.AdminString);
        }

        public async Task<string> MakeModerator(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || await this.userManager.IsInRoleAsync(user, Constants.Strings.ModeratorRole))
            {
                return "";
            }

            await this.userManager.RemoveFromRoleAsync(user, Constants.Strings.UserRole);
            await this.userManager.AddToRoleAsync(user, Constants.Strings.ModeratorRole);

            return $"{user.UserName} {Constants.Messages.PromotedUserMsg}";
        }

        public async Task<string> DemoteFromModerator(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || await this.userManager.IsInRoleAsync(user, Constants.Strings.UserRole))
            {
                return "";
            }

            await this.userManager.RemoveFromRoleAsync(user, Constants.Strings.ModeratorRole);
            await this.userManager.AddToRoleAsync(user, Constants.Strings.UserRole);

            return $"{user.UserName} {Constants.Messages.DemotedUserMsg}";
        }

        public async Task<bool> SignInUser(LoginViewModel bindingModel)
        {
            var result = await signInManager.PasswordSignInAsync(bindingModel.Username,
                bindingModel.Password, bindingModel.RememberMe, false);

            return result.Succeeded;
        }
    }
}
