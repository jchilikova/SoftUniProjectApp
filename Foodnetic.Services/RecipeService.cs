using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Ingredients;
using Foodnetic.ViewModels.Recipes;
using Microsoft.EntityFrameworkCore;

namespace Foodnetic.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly FoodneticDbContext dbContext;

        public RecipeService(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateIngredient(CreateIngredientViewModel bindingModel)
        {
            Recipe recipe = this.dbContext.Recipes.FirstOrDefault(x => x.IsInCreate);

            var ingredient = new Ingredient
            {
                Name = bindingModel.Name,
                Quantity = bindingModel.Quantity,
                Recipe = recipe
            };

            this.dbContext.Ingredients.Add(ingredient);
            recipe.Ingredients.Add(ingredient);
            this.dbContext.SaveChanges();
        }

        public void DeleteRecipe(string id)
        {
            var recipe = this.dbContext.Recipes.FirstOrDefault(x => x.Id == id);
            recipe.IsDeleted = true;
            this.dbContext.SaveChanges();
        }

        public IEnumerable<Recipe> GetAll()
        {
            if (this.dbContext.Recipes.Any())
            {
                var recipes = this.dbContext.Recipes.Include(x => x.Stars).Include(r => r.Author)
                    .Where(x => x.IsDeleted == false && x.IsInCreate == false);
                return recipes;
            }

            return null;
        }

        public Recipe GetById(string id)
        {
            var recipe = this.dbContext.Recipes
                .Include(x => x.Stars)
                .Include(x => x.Author)
                .Include(x => x.Comments)
                .ThenInclude(x => x.Author)
                .Include(x => x.Ingredients)
                .Where(x => x.IsDeleted == false)
                .FirstOrDefault(r => r.Id == id);

            return recipe;
        }

        public ICollection<IngredientsViewModel> GetIngredients()
        {
            var bindingModels = new List<IngredientsViewModel>();

            Recipe recipe;

            if (this.dbContext.Recipes.Any(x => x.IsInCreate == true))
            {
                recipe = this.dbContext.Recipes.Include(x => x.Ingredients).FirstOrDefault(x => x.IsInCreate);
                foreach (var recipeIngredient in recipe.Ingredients)
                {
                    bindingModels.Add(new IngredientsViewModel
                    {
                        Name = recipeIngredient.Name,
                        Quantity = recipeIngredient.Quantity
                    });
                }

                return bindingModels;
            }
            else
            {
                recipe = new Recipe
                {
                    IsInCreate = true,
                };

                this.dbContext.Recipes.Add(recipe);
                this.dbContext.SaveChanges();

                return null;
            }

        }

        public void CreateRecipe(CreateRecipeViewModel bindingModel, string username)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == username)?.Id;
            var recipe = this.dbContext.Recipes.Include(x => x.Ingredients).FirstOrDefault(x => x.IsInCreate);

            recipe.AuthorId = userId;
            recipe.PreparationTime = bindingModel.PreparationTime;
            recipe.CookTime = bindingModel.CookTime;
            recipe.IsInCreate = false;
            recipe.Description = bindingModel.Description;
            recipe.Directions = bindingModel.Directions;
            recipe.Name = bindingModel.Name;
            recipe.NumberOfServings = bindingModel.NumberOfServings;

            var imageModel = bindingModel.Image;

            if (imageModel?.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageModel.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    recipe.Image = fileBytes;
                }
            }

            this.dbContext.SaveChanges();
        }

        public bool RecipeExists(string id)
        {
            return this.dbContext.Recipes.Any(x => x.Id == id && x.IsDeleted == false);
        }
    }
}
