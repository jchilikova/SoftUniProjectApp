using System.Collections.Generic;
using Foodnetic.Models;
using Foodnetic.ViewModels.Recipes;

namespace Foodnetic.Services.Contracts
{
    public interface IRecipeService
    {
        IEnumerable<Recipe> GetAll();

        Recipe GetById(string id);
        bool RecipeExists(string id);
        void DeleteRecipe(string id);
    }
}
