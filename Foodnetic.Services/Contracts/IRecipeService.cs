using System.Collections.Generic;
using Foodnetic.ViewModels.Recipes;

namespace Foodnetic.Services.Contracts
{
    public interface IRecipeService
    {
        IEnumerable<AllRecipesViewModel> GetAll();

        RecipeViewModel GetById(string id);
    }
}
