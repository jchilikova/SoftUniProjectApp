using System;
using System.Linq;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Tests.MenuServiceTests
{
    public class MenuServiceTests : BaseService
    {
        private IMenuService MenuService => this.ServiceProvider.GetRequiredService<IMenuService>();

        [Test]
        public void GetAllMenusForUserShouldReturnAllMenusWithSaidUsername()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test"
            };

            var menu = new Menu
            {
                Id = "testMenu",
                UserId = "test"
            };

            var menu2 = new Menu
            {
                Id = "testMenu2",
                UserId = "test"
            };

            this.DbContext.Menus.Add(menu);
            this.DbContext.Menus.Add(menu2);

            user.DailyMenus.Add(menu);
            user.DailyMenus.Add(menu2);

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.MenuService.GetAllMenusForUser("Test").Count;

            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetAllMenusForUserShouldReturnNullIfHasNoMenus()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.MenuService.GetAllMenusForUser("Test");

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetDailyMenuForUserShouldReturnMenuWithSaidUsername()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test"
            };

            var menu = new Menu
            {
                Id = "testMenu",
                UserId = "test",
                CreatedOn = DateTime.Today
            };

            this.DbContext.Menus.Add(menu);
            user.DailyMenus.Add(menu);

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.MenuService.GetDailyMenuForUser("Test");

            Assert.AreEqual(result, menu);
        }

        [Test]
        public void CheckIfMenuForUserExistsShouldReturnTrueIfUserHasMenuForToday()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test"
            };

            var menu = new Menu
            {
                Id = "testMenu",
                UserId = "test",
                CreatedOn = DateTime.Today
            };

            this.DbContext.Menus.Add(menu);
            user.DailyMenus.Add(menu);

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.MenuService.CheckIfMenuExist("Test");

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckIfMenuForUserExistsShouldReturnFalseIfUserHasNoMenuForToday()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test"
            };

            var menu = new Menu
            {
                Id = "testMenu",
                UserId = "test",
                CreatedOn = DateTime.Today.AddDays(-3)
            };

            this.DbContext.Menus.Add(menu);
            user.DailyMenus.Add(menu);

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.MenuService.CheckIfMenuExist("Test");

            Assert.IsFalse(result);
        }

        [Test]
        public void CreateMenuShouldAddMenuToDatabaseForSpecificUserAndReturnIt()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test",
                VirtualFridge = new VirtualFridge()
            };

            user.VirtualFridge.Groceries.Add(new Grocery
            {
                Name = "tomato",
                Quantity = 200,
                ProductType = ProductType.Fruits,
                ExpirationDate = DateTime.MaxValue
            });

            var recipe = new Recipe
            {
                Id = "test",
                DishType = DishType.Breakfast
            };

            var tag = new Tag
            {
                Name = "breakfast",
                Id = "test"
            };

            this.DbContext.Tags.Add(tag);

            var recipeTag = new RecipeTag
            {
                RecipeId = "test",
                TagId = "test"
            };

            this.DbContext.RecipeTags.Add(recipeTag);

            recipe.Ingredients.Add(new Ingredient
            {
                Name = "tomato",
                Quantity = 100,
            });

            this.DbContext.Users.Add(user);
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var result = this.MenuService.Create("Test");

            Assert.AreEqual(result.RecipeMenus.Count, 1);
        }

        [Test]
        public void CreateMenuShouldAddMenuToDatabaseForSpecificUserAndReturnNullRecipeMenusIfUserDoesNotHaveEnoughIngredients()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test",
                VirtualFridge = new VirtualFridge()
            };

            user.VirtualFridge.Groceries.Add(new Grocery
            {
                Name = "tomato",
                Quantity = 200,
                ProductType = ProductType.Fruits,
                ExpirationDate = DateTime.MaxValue
            });

            var recipe = new Recipe
            {
                Id = "test"
            };

            var tag = new Tag
            {
                Name = "breakfast",
                Id = "test"
            };

            this.DbContext.Tags.Add(tag);

            var recipeTag = new RecipeTag
            {
                RecipeId = "test",
                TagId = "test"
            };

            this.DbContext.RecipeTags.Add(recipeTag);

            recipe.Ingredients.Add(new Ingredient
            {
                Name = "tomato",
                Quantity = 500,
            });

            this.DbContext.Users.Add(user);
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var result = this.MenuService.Create("Test");

            Assert.AreEqual(0, result.RecipeMenus.Count);
        }

        [Test]
        public void CreateMenuShouldAddMenuToDatabaseForSpecificUserReturnItAndDecreaseGroceriesOfUser()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "Test",
                VirtualFridge = new VirtualFridge()
            };

            user.VirtualFridge.Groceries.Add(new Grocery
            {
                Name = "tomato",
                Quantity = 200,
                ProductType = ProductType.Fruits,
                ExpirationDate = DateTime.MaxValue
            });

            var recipe = new Recipe
            {
                Id = "test"
            };

            var tag = new Tag
            {
                Name = "breakfast",
                Id = "test"
            };

            this.DbContext.Tags.Add(tag);

            var recipeTag = new RecipeTag
            {
                RecipeId = "test",
                TagId = "test"
            };

            this.DbContext.RecipeTags.Add(recipeTag);

            recipe.Ingredients.Add(new Ingredient
            {
                Name = "tomato",
                Quantity = 100,
            });

            this.DbContext.Users.Add(user);
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var result = this.MenuService.Create("Test");
            var userGroceryQuantity = user.VirtualFridge.Groceries.FirstOrDefault(x => x.Name == "tomato").Quantity;

            Assert.AreEqual(100, userGroceryQuantity);
        }
    }
}

