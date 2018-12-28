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
            Recipe recipe = this.dbContext.Recipes.Include(x => x.Ingredients).FirstOrDefault(x => x.IsInCreate);
            Ingredient ingredient;

            if (recipe.Ingredients.Any(x => x.Name == bindingModel.Name))
            {
                ingredient = recipe.Ingredients.FirstOrDefault(x => x.Name == bindingModel.Name);
                ingredient.Quantity += bindingModel.Quantity;
            }
            else
            {
                ingredient = new Ingredient
                {
                    Name = bindingModel.Name,
                    Quantity = bindingModel.Quantity,
                    Recipe = recipe
                };

                this.dbContext.Ingredients.Add(ingredient);
                recipe.Ingredients.Add(ingredient);
            }
          
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
            recipe.DishType = bindingModel.DishType;

            this.AddTagsToRecipe(bindingModel, recipe);
            this.AddImageToRecipe(bindingModel, recipe);

            this.dbContext.SaveChanges();
        }

        private void AddImageToRecipe(CreateRecipeViewModel bindingModel, Recipe recipe)
        {
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

        private void AddTagsToRecipe(CreateRecipeViewModel bindingModel, Recipe recipe)
        {
            var tagStrings = bindingModel
                .Tags
                .Split(new char[] {' ', ',', ';', '.', '-'}, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var recipeTags = new List<RecipeTag>();

            foreach (var tagString in tagStrings)
            {
                var id = Guid.NewGuid().ToString();

                var tag = new Tag
                {
                    Name = tagString,
                    Id = id
                };

                if (this.dbContext.Tags.Any(x => x.Name == tagString))
                {
                    id = this.dbContext.Tags.FirstOrDefault(x => x.Name == tagString)?.Id;
                }
                else
                {
                    this.dbContext.Tags.Add(tag);
                    this.dbContext.SaveChanges();
                }

                recipeTags.Add(new RecipeTag
                {
                    Recipe = recipe,
                    TagId = id
                });

            }

            this.dbContext.RecipeTags.AddRange(recipeTags);
            this.dbContext.SaveChanges();
        }

        public bool RecipeExists(string id)
        {
            return this.dbContext.Recipes.Any(x => x.Id == id && x.IsDeleted == false);
        }

        public void CancelRecipe()
        {
            var recipe = this.dbContext.Recipes.Include(x => x.Ingredients).FirstOrDefault(x => x.IsInCreate);
            if (recipe == null)
            {
                return;
            }

            var ingredients = this.dbContext.Ingredients.Where(x => x.RecipeId == null || x.RecipeId == recipe.Id);
            
            this.dbContext.Ingredients.RemoveRange(ingredients);
            this.dbContext.Recipes.Remove(recipe);
            this.dbContext.SaveChanges();
        }
    }
}
