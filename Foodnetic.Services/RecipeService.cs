using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Data;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Recipes;
using Microsoft.EntityFrameworkCore;

namespace Foodnetic.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly FoodneticDbContext dbContext;
        private readonly IMapper mapper;

        public RecipeService(FoodneticDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IEnumerable<AllRecipesViewModel> GetAll()
        {
            var recipes = this.dbContext.Recipes.Include(r => r.Author);
            var recipeModels = new List<AllRecipesViewModel>();

            foreach (var recipe in recipes)
            {
                var model = this.mapper.Map<AllRecipesViewModel>(recipe);

                recipeModels.Add(model);
            }

            return recipeModels;
        }

        public RecipeViewModel GetById(string id)
        {
            var recipe = this.dbContext.Recipes.Include(x => x.Author).Include(x => x.Comments).FirstOrDefault(r => r.Id == id);

            var recipeModel = this.mapper.Map<RecipeViewModel>(recipe);

            return recipeModel;
        }
    }
}
