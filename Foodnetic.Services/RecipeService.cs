using System.Collections.Generic;
using System.Linq;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
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

        public void DeleteRecipe(string id)
        {
            var recipe = this.dbContext.Recipes.FirstOrDefault(x => x.Id == id);
            recipe.IsDeleted = true;
            this.dbContext.SaveChanges();
        }

        public IEnumerable<Recipe> GetAll()
        {
            var recipes = this.dbContext.Recipes.Include(x => x.Stars).Include(r => r.Author).Where(x => x.IsDeleted == false);

            return recipes;
        }

        public Recipe GetById(string id)
        {
            var recipe = this.dbContext.Recipes
                    .Include(x => x.Stars)
                .Include(x => x.Author)
                .Include(x => x.Comments)
                .ThenInclude(x => x.Author)
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .Where(x => x.IsDeleted == false)
                .FirstOrDefault(r => r.Id == id);

            return recipe;
        }

        public bool RecipeExists(string id)
        {
            return this.dbContext.Recipes.Any(x => x.Id == id && x.IsDeleted == false);
        }
    }
}
