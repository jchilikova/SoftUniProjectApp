using System;
using System.Linq;
using System.Threading.Tasks;
using Foodnetic.Constants;
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
            UserManager<FoodneticUser> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await SeedRoles(userManager, roleManager);
            }
          
            await next(context);
        }

        private async Task SeedRoles(
            UserManager<FoodneticUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name =  Constants.Constants.Strings.AdministratorRole
            });

            await roleManager.CreateAsync(new IdentityRole
            {
                Name =  Constants.Constants.Strings.UserRole
            });

            await roleManager.CreateAsync(new IdentityRole
            {
                Name =  Constants.Constants.Strings.ModeratorRole
            });

            var user = new FoodneticUser
            {
                UserName = AdminString,
                FirstName = AdminString,
                LastName = AdminString,
                Email = AdminEmail,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            await userManager.CreateAsync(user, AdminString);

            await userManager.AddToRoleAsync(user, Constants.Constants.Strings.AdministratorRole);
        }
    }
}

