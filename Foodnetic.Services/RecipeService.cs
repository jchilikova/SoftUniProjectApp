using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
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

        public IEnumerable<Recipe> GetAll()
        {
            var recipes = this.dbContext.Recipes.Include(x => x.Stars).Include(r => r.Author);

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
                .FirstOrDefault(r => r.Id == id);

            return recipe;
        }

        public bool RecipeExists(string id)
        {
            return this.dbContext.Recipes.Any(x => x.Id == id);
        }
    }
}
