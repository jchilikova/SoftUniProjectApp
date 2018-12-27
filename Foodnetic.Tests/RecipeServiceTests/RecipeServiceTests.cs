using System.IO;
using System.Linq;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Ingredients;
using Foodnetic.ViewModels.Recipes;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Tests.RecipeServiceTests
{
    public class RecipeServiceTests : BaseService
    {
        private IRecipeService RecipeService => this.ServiceProvider.GetRequiredService<IRecipeService>();


        [Test]
        public void CreateIngredientShouldAddIngredientToRecipe()
        {
            var recipe = new Recipe
            {
                Id = "testRecipe",
                IsInCreate = true
            };

            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var bindingModel = new CreateIngredientViewModel
            {
                Name = "test",
                Quantity = 100
            };

            this.RecipeService.CreateIngredient(bindingModel);

            var result = this.DbContext.Recipes.
                Include(x => x.Ingredients)
                .FirstOrDefault(x => x.IsInCreate).Ingredients.Count;


            Assert.AreEqual(result, 1);
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

            var result = this.DbContext.Recipes.FirstOrDefault(x => x.Id == "test").IsDeleted;

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
            var recipe = new Recipe
            {
                Id = "test",
                IsInCreate = true
            };
            recipe.Ingredients.Add(new Ingredient());
            recipe.Ingredients.Add(new Ingredient());
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var result = this.RecipeService.GetIngredients().Count;

            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetIngredientsShouldReturnNullIfRecipeDoesNotExists()
        {
            var result = this.RecipeService.GetIngredients();

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

            this.DbContext.Recipes.Add(new Recipe
            {
                Id = "2"
            });

            this.DbContext.Recipes.Add(new Recipe
            {
                Id = "1",
                IsInCreate = true
            });

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var bindingModel = new CreateRecipeViewModel
            {
                CookTime = 10,
                PreparationTime = 10,
                Description = "test",
                Directions = "test",
                Name = "test",
                NumberOfServings = 2
            };

            this.RecipeService.CreateRecipe(bindingModel, "test");

            var result = this.DbContext.Recipes.Count(x => x.IsInCreate == false);

            Assert.AreEqual(2, result);
        }
    }
}
