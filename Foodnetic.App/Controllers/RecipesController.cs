using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using Foodnetic.Contants;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Comments;
using Foodnetic.ViewModels.Ingredients;
using Foodnetic.ViewModels.Products;
using Foodnetic.ViewModels.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Foodnetic.App.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipeService recipeService;
        private readonly IMapper mapper;

        public RecipesController(IRecipeService recipeService, IMapper mapper)
        {
            this.recipeService = recipeService;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult All(string data, int? page)
        {
            this.ViewData[Constants.Strings.ErrorString] = data;

            var recipes = this.recipeService.GetAll();

            var recipeModels = this.MapAllRecipes(recipes);

            var nextPage = page ?? 1;

            var pageViewModel = recipeModels.ToPagedList(nextPage, 6);

            return this.View(pageViewModel);
        }

        private IEnumerable<AllRecipesViewModel> MapAllRecipes(IEnumerable<Recipe> recipes)
        {
            var recipeModels = new List<AllRecipesViewModel>();

            foreach (var recipe in recipes)
            {
                var model = this.mapper.Map<AllRecipesViewModel>(recipe);

                recipeModels.Add(model);
            }

            return recipeModels;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();

        }

        //[HttpPost]
        //[Authorize]
        //TODO
        //public IActionResult Create()
        //{
        //    return this.View();
        //}

        [Authorize]
        public IActionResult Recipe(string id)
        {
            if (this.recipeService.RecipeExists(id))
            {
                var recipe = this.recipeService.GetById(id);

                var recipeModel = this.MapSingleRecipe(recipe);

                return this.View(recipeModel);
            }

            var data = Constants.Messages.InvalidRecipeMsgError;

            return RedirectToAction("All", new {Data = data});
        }

        private RecipeViewModel MapSingleRecipe(Recipe recipe)
        {
            var recipeModel = this.mapper.Map<RecipeViewModel>(recipe);

            foreach (var ingredient in recipe.Ingredients)
            {
                recipeModel.IngredientsViewModel.Add(new IngredientsViewModel
                {
                    Name = ingredient.Ingredient.Name,
                    Quantity = ingredient.Ingredient.Quantity
                });
            }

            foreach (var comment in recipe.Comments)
            {
                recipeModel.CommentViewModels.Add(new CommentViewModel
                {
                    Content = comment.Content,
                    PostedOn = comment.PostedOn.ToString("dd-MM-yyy hh:mm",CultureInfo.InvariantCulture),
                    RecipeId = comment.RecipeId,
                    Username = comment.Author.UserName,
                    Id=comment.Id
                });
            }

            return recipeModel;
        }
    }
}
