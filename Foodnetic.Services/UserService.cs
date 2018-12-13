using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> signInManager;
        private readonly FoodneticDbContext dbContext;
        private readonly IMapper mapper;

        public UserService(SignInManager<User> signInManager, FoodneticDbContext dbContext, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.mapper = mapper;
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
                        var roleResult = this.signInManager.UserManager.AddToRoleAsync(user, "User").Result;

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
    }
}
