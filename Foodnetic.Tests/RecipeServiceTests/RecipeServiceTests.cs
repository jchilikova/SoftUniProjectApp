using System.Linq;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Ingredients;
using Foodnetic.ViewModels.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Services.Tests.RecipeServiceTests
{
    public class RecipeServiceTests : BaseService
    {
        private IRecipeService RecipeService => this.ServiceProvider.GetRequiredService<IRecipeService>();


        [Test]
        public void CreateIngredientShouldAddIngredientToRecipe()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "test"
            };

            var recipe = new Recipe
            {
                Id = "testRecipe",
                IsInCreate = true,
                AuthorId = user.Id
            };

            this.DbContext.Users.Add(user);
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var bindingModel = new CreateIngredientViewModel
            {
                Name = "test",
                Quantity = 100
            };

            this.RecipeService.CreateIngredient(bindingModel, user.UserName);

            var result = this.DbContext.Recipes.
                Include(x => x.Ingredients)
                .FirstOrDefault(x => x.IsInCreate).Ingredients.Count;


            Assert.AreEqual(result, 1);
        }

        [Test]
        public void CancelRecipeShouldDeleteInCreateRecipe()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "test"
            };

            var recipe = new Recipe
            {
                Id = "testRecipe",
                IsInCreate = true,
                AuthorId = user.Id
            };

            this.DbContext.Users.Add(user);
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            this.RecipeService.CancelRecipe(user.UserName);

            var result = this.DbContext.Recipes.Any(x => x.IsInCreate && x.AuthorId == "test");

            Assert.IsFalse(result);
        }


        [Test]
        public void DeleteRecipeShouldSetIsDeletedToTrue()
        {
            var recipe = new Recipe
            {
                Id = "test"
            };

            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            this.RecipeService.DeleteRecipe("test");

            var result = this.DbContext.Recipes.FirstOrDefault(x => x.Id == "test")?.IsDeleted;

            Assert.IsTrue(result);
        }

        [Test]
        public void GetAllRecipesShouldReturnAllRecipesNotDeleted()
        {
            var recipe = new Recipe
            {
                Id = "test"
            };

            var recipe2 = new Recipe
            {
                Id = "test2"
            };

            this.DbContext.Recipes.Add(recipe);
            this.DbContext.Recipes.Add(recipe2);
            this.DbContext.SaveChanges();

            var result = this.RecipeService.GetAll().Count();

            Assert.AreEqual(result, 2);
        }


        [Test]
        public void GetByIdShouldReturnRecipeById()
        {
            var recipe = new Recipe
            {
                Id = "test",
                CookTime = 10,
                Description = "testTest",
                Directions = "1. Test",
                PreparationTime = 100,
                NumberOfServings = 4
            };

            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var recipeResult = this.RecipeService.GetById("test");

            Assert.AreEqual(recipe, recipeResult);
        }

        [Test]
        public void GetIngredientsShouldReturnAllRecipeIngredients()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var recipe = new Recipe
            {
                Id = "test",
                IsInCreate = true,
                AuthorId = user.Id
            };

            recipe.Ingredients.Add(new Ingredient());
            recipe.Ingredients.Add(new Ingredient());
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var result = this.RecipeService.GetIngredients(user.UserName).Count;

            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetIngredientsShouldReturnNullIfRecipeDoesNotExists()
        {
            var user = new FoodneticUser
            {
                Id = "test",
                UserName = "test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.RecipeService.GetIngredients((user.UserName));

            Assert.AreEqual(null, result);
        }

        [Test]
        public void CheckIfRecipeExistsShouldReturnTrueIfExists()
        {
            var recipe = new Recipe
            {
                Id = "test"
            };

            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var result = this.RecipeService.RecipeExists("test");

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckIfRecipeExistsShouldReturnFalseIfRecipeNotExists()
        {
            var result = this.RecipeService.RecipeExists("test");

            Assert.IsFalse(result);
        }

        [Test]
        public void CreateRecipeShouldAddRecipeToDatabase()
        {
            var user = new FoodneticUser
            {
                Id = "1",
                UserName = "test"
            };

            var user2 = new FoodneticUser
            {
                Id = "2",
                UserName = "test"
            };
            this.DbContext.Users.Add(user);
            this.DbContext.Users.Add(user2);
            this.DbContext.SaveChanges();

            this.DbContext.Recipes.Add(new Recipe
            {
                Id = "2",
                IsInCreate = true,
                AuthorId = "2"
            });

            this.DbContext.Recipes.Add(new Recipe
            {
                Id = "1",
                IsInCreate = true,
                AuthorId = "1"
            });

            this.DbContext.SaveChanges();

            var bindingModel = new CreateRecipeViewModel
            {
                CookTime = 10,
                PreparationTime = 10,
                Description = "test",
                Directions = "test",
                Name = "test",
                NumberOfServings = 2,
                Tags = "test, breakfast",
                DishType = DishType.Breakfast
            };

            this.RecipeService.CreateRecipe(bindingModel, "test");

            var result = this.DbContext.Recipes.Count(x => x.IsInCreate == false);

            Assert.AreEqual(1, result);
        }
    }
}
