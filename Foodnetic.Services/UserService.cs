using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Foodnetic.Constants;
using Foodnetic.Infrastructure;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<FoodneticUser> signInManager;
        private readonly UserManager<FoodneticUser> userManager;
        private readonly IMapper mapper;

        public UserService(SignInManager<FoodneticUser> signInManager, IMapper mapper, UserManager<FoodneticUser> userManager)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task AddToUserRole(FoodneticUser user)
        {
            await this.signInManager.UserManager.AddToRoleAsync(user, GlobalConstants.UserRole);
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

        public FoodneticUser RegisterUser(RegisterViewModel bindingModel)
        {
            var user = this.mapper.Map<FoodneticUser>(bindingModel);

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
                user = new FoodneticUser
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
                        var roleResult = this.signInManager.UserManager.AddToRoleAsync(user, GlobalConstants.UserRole).Result;

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

        public IEnumerable<FoodneticUser> GetAll()
        {
            return this.signInManager.UserManager.Users.Where(x => x.UserName != GlobalConstants.AdminString);
        }

        public async Task<string> MakeModerator(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || await this.userManager.IsInRoleAsync(user, GlobalConstants.ModeratorRole))
            {
                return "";
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.UserRole);
            await this.userManager.AddToRoleAsync(user, GlobalConstants.ModeratorRole);

            return $"{user.UserName} {ConstantMessages.PromotedUserMsg}";
        }

        public async Task<string> DemoteFromModerator(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || await this.userManager.IsInRoleAsync(user, GlobalConstants.UserRole))
            {
                return "";
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.ModeratorRole);
            await this.userManager.AddToRoleAsync(user, GlobalConstants.UserRole);

            return $"{user.UserName} {ConstantMessages.DemotedUserMsg}";
        }

        public SignInResult SignInUser(LoginViewModel bindingModel)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.UserName == bindingModel.Username);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            var password = bindingModel.Password;
            var result = this.signInManager
                .PasswordSignInAsync(user, password, bindingModel.RememberMe, false).Result;

            return result;
        }
    }
}
