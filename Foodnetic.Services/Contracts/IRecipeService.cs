﻿using System.Collections.Generic;
using Foodnetic.Models;
using Foodnetic.ViewModels.Ingredients;
using Foodnetic.ViewModels.Recipes;

namespace Foodnetic.Services.Contracts
{
    public interface IRecipeService
    {
        IEnumerable<Recipe> GetAll();

        Recipe GetById(string id);

        bool RecipeExists(string id);

        void DeleteRecipe(string id);

        void CreateIngredient(CreateIngredientViewModel bindingModel, string username);

        ICollection<IngredientsViewModel> GetIngredients(string username);

        void CreateRecipe(CreateRecipeViewModel bindingModel, string username);

        void CancelRecipe(string username);
    }
}
