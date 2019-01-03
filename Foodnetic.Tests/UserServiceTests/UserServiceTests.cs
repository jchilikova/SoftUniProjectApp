using System.Linq;
using System.Threading.Tasks;
using Foodnetic.Constants;
using Foodnetic.Infrastructure;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Services.Tests.UserServiceTests
{
    public class UserServiceTests : BaseService
    {
        protected UserManager<FoodneticUser> UserManager => this.ServiceProvider.GetRequiredService<UserManager<FoodneticUser>>();
        protected RoleManager<IdentityRole> RoleManager => this.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      protected IUserService UserService => this.ServiceProvider.GetRequiredService<IUserService>();


        [Test]
        public void CreateUserShouldReturnUserIfSucceeded()
        {
            var registerBindingModel = new RegisterViewModel
            {
                FirstName = "test",
                LastName = "test",
                Email = "test@test.test",
                Password = "123456",
                ConfirmPassword = "123456",
                Username = "test123"
            };

            var result = this.UserService.RegisterUser(registerBindingModel);

            var user = this.DbContext.Users.FirstOrDefault();

            Assert.AreEqual(result, user);
        }

        [Test]
        public void LogInShouldReturnTrueIfSucceeded()
        {
            var user = new FoodneticUser()
            {
                UserName = "Test",
            };

            var userPassword = "test";

            this.UserManager.CreateAsync(user).GetAwaiter().GetResult();
            this.UserManager.AddPasswordAsync(user, userPassword).GetAwaiter().GetResult();
            this.DbContext.SaveChanges();


            var loginViewModel = new LoginViewModel
            {
                Username = "Test",
                Password = userPassword,
                RememberMe = true
            };

            var result = this.UserService.SignInUser(loginViewModel);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public void LogInShouldReturnFalseIfNotSucceeded()
        {
            var loginViewModel = new LoginViewModel
            {
                Username = "test",
                Password = "test",
                RememberMe = true
            };

            var result = this.UserService.SignInUser(loginViewModel);

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void CheckIfUserExistsShouldReturnTrueIfUserExists()
        {
            var user = new FoodneticUser
            {
                UserName = "test",
                Id = "test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.UserService.CheckIfUserExists("test");

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckIfUserExistsShouldReturnFalseIfUserDoesNotExists()
        {
            var user = new FoodneticUser
            {
                UserName = "no",
                Id = "no"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.UserService.CheckIfUserExists("test");

            Assert.IsFalse(result);
        }

        [Test]
        public void GetAllUsersShouldReturnAllUsersExceptAdmins()
        {
            var user = new FoodneticUser
            {
                UserName = "no",
                Id = "1"
            };

            var user2 = new FoodneticUser
            {
                UserName = "no",
                Id = "2"
            };

            var user3 = new FoodneticUser
            {
                UserName = "no",
                Id = "3"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.Users.Add(user2);
            this.DbContext.Users.Add(user3);
            this.DbContext.SaveChanges();

            var result = this.UserService.GetAll();

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task DemoteFromModeratorShouldMakeModeratorToUserRole()
        {
            var user = new FoodneticUser
            {
                Id="test",
                UserName = "test"
            };

            var userPassword = "test";

            await RoleManager.CreateAsync(new IdentityRole
            {
                Name =  GlobalConstants.UserRole
            });

            await RoleManager.CreateAsync(new IdentityRole
            {
                Name =  GlobalConstants.ModeratorRole
            });

            this.UserManager.CreateAsync(user).GetAwaiter().GetResult();
            this.UserManager.AddPasswordAsync(user, userPassword).GetAwaiter().GetResult();
            await this.UserManager.AddToRoleAsync(user,  GlobalConstants.ModeratorRole);

            var result = this.UserService.DemoteFromModerator("test");

            Assert.AreEqual($"test {ConstantMessages.DemotedUserMsg}", result.Result);
        }

        [Test]
        public async Task MakeModeratorShouldMakeUserWithUserRoleToModerator()
        {
            var user = new FoodneticUser
            {
                Id="test",
                UserName = "test"
            };

            var userPassword = "test";

            await RoleManager.CreateAsync(new IdentityRole
            {
                Name =  GlobalConstants.UserRole
            });

            await RoleManager.CreateAsync(new IdentityRole
            {
                Name =  GlobalConstants.ModeratorRole
            });

            this.UserManager.CreateAsync(user).GetAwaiter().GetResult();
            this.UserManager.AddPasswordAsync(user, userPassword).GetAwaiter().GetResult();
            this.DbContext.SaveChanges();
            await this.UserManager.AddToRoleAsync(user,  GlobalConstants.UserRole);
            this.DbContext.SaveChanges();

            var result = this.UserService.MakeModerator("test");

            Assert.AreEqual($"test {ConstantMessages.PromotedUserMsg}", result.Result);
        }

    }
}
