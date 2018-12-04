using System;
using System.Linq;
using System.Threading.Tasks;
using Foodnetic.App.Globals;
using Foodnetic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.App.Middlewares
{
    public class SeedRolesMiddleware
    {
        private readonly RequestDelegate next;
        private const string AdminString = "admin";
        private const string AdminEmail = "admin@admin.com";
    
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
                await SeedRoles(userManager, roleManager);
            }
          
            await next(context);
        }

        private async Task SeedRoles(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name =  GlobalConstants.AdministratorRole
            });

            await roleManager.CreateAsync(new IdentityRole
            {
                Name =  GlobalConstants.UserRole
            });

            var user = new User
            {
                UserName = AdminString,
                FirstName = AdminString,
                LastName = AdminString,
                Email = AdminEmail,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await userManager.CreateAsync(user, AdminString);

            await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRole);
        }
    }
}

