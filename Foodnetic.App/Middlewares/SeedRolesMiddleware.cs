using System.Linq;
using System.Threading.Tasks;
using Foodnetic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.App.Middlewares
{
    public class SeedRolesMiddleware
    {
        private readonly RequestDelegate next;
        private const string AdministratorRole = "Administrator";
        private const int UsersCount = 1;
    
        public SeedRolesMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            UserManager<User> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await SeedRoles(roleManager);
            }

            if (userManager.Users.Count() == UsersCount)
            {
                await this.SetAdministratorRole(userManager);
            }
          
            await next(context);
        }

        private async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = AdministratorRole
            });
        }

        private async Task SetAdministratorRole(UserManager<User> userManager)
        {
            var user = userManager.Users.FirstOrDefault();

            await userManager.AddToRoleAsync(user, AdministratorRole);
        }
    }
}

