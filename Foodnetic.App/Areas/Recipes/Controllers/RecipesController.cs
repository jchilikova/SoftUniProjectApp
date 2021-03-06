﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Foodnetic.Constants;
using Foodnetic.Infrastructure;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Comments;
using Foodnetic.ViewModels.Ingredients;
using Foodnetic.ViewModels.Products;
using Foodnetic.ViewModels.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Foodnetic.App.Areas.Recipes.Controllers
{
    [Area(GlobalConstants.RecipesAreaString)]
    public class RecipesController : Controller
    {
        private const string ImageString = "data:image/jpg;base64,{0}";
        private readonly IRecipeService recipeService;
        private readonly IMapper mapper;
        private readonly IProductService productService;

        public RecipesController(IRecipeService recipeService, IMapper mapper, IProductService productService)
        {
            this.recipeService = recipeService;
            this.mapper = mapper;
            this.productService = productService;
        }

        [Authorize]
        public IActionResult All(string data, int? page)
        {
            this.ViewData[GlobalConstants.ErrorString] = data;
        
            var recipes = this.recipeService.GetAll();

            if (recipes == null)
            {
                return this.View();
            }

            var orderedRecipes = recipes.OrderByDescending(x => x.Rating).ThenByDescending(x => x.Comments.Count)
                .ThenBy(x => x.Name);

            var recipeModels = this.MapAllRecipes(orderedRecipes);

            var nextPage = page ?? 1;

            var pageViewModel = recipeModels.ToPagedList(nextPage, 6);

            return this.View(pageViewModel);
        }

        [Authorize]
        public IActionResult AllBreakfastRecipes(int? page)
        {
            var recipes = this.recipeService.GetAll();

            if (recipes == null)
            {
                return this.View();
            }

            var recipesOrdered = recipes.Where(x => x.DishType == DishType.Breakfast).OrderBy(x => x.Rating);

            var recipeModels = this.MapAllRecipes(recipesOrdered);

            var nextPage = page ?? 1;

            var pageViewModel = recipeModels.ToPagedList(nextPage, 6);

            return this.View(pageViewModel);
        }

        [Authorize]
        public IActionResult AllLunchRecipes(int? page)
        {
            var recipes = this.recipeService.GetAll();

            if (recipes == null)
            {
                return this.View();
            }

            var recipesOrdered = recipes.Where(x => x.DishType == DishType.Lunch).OrderBy(x => x.Rating);

            var recipeModels = this.MapAllRecipes(recipesOrdered);

            var nextPage = page ?? 1;

            var pageViewModel = recipeModels.ToPagedList(nextPage, 6);

            return this.View(pageViewModel);
        }

        [Authorize]
        public IActionResult AllDinnerRecipes(int? page)
        {
            var recipes = this.recipeService.GetAll();

            if (recipes == null)
            {
                return this.View();
            }

            var recipesOrdered = recipes.Where(x => x.DishType == DishType.Dinner).OrderBy(x => x.Rating);

            var recipeModels = this.MapAllRecipes(recipesOrdered);

            var nextPage = page ?? 1;

            var pageViewModel = recipeModels.ToPagedList(nextPage, 6);

            return this.View(pageViewModel);
        }

        [Authorize]
        public IActionResult AllDessertRecipes(int? page)
        {
            var recipes = this.recipeService.GetAll();

            if (recipes == null)
            {
                return this.View();
            }

            var recipesOrdered = recipes.Where(x => x.DishType == DishType.Dessert).OrderBy(x => x.Rating);

            var recipeModels = this.MapAllRecipes(recipesOrdered);

            var nextPage = page ?? 1;

            var pageViewModel = recipeModels.ToPagedList(nextPage, 9);

            return this.View(pageViewModel);
        }

        [Authorize]
        public IActionResult Cancel()
        {
            var username = this.User.Identity.Name;
            this.recipeService.CancelRecipe(username);

            return this.RedirectToAction("All");
        }

        private IEnumerable<AllRecipesViewModel> MapAllRecipes(IEnumerable<Recipe> recipes)
        {
            var recipeModels = new List<AllRecipesViewModel>();

            foreach (var recipe in recipes)
            {
                var model = this.mapper.Map<AllRecipesViewModel>(recipe);
                var base64 = Convert.ToBase64String(recipe.Image);
                var imgSrc = $"data:image/jpg;base64,{base64}";
                model.PictureUrl = imgSrc;

                recipeModels.Add(model);
            }

            return recipeModels;
        }

        [Authorize]
        public IActionResult Create()
        {
            var username = this.User.Identity.Name;
            CreateRecipeViewModel bindingModel =
                new CreateRecipeViewModel {IngredientsViewModels = this.recipeService.GetIngredients(username)};

            if (bindingModel.IngredientsViewModels == null || bindingModel.IngredientsViewModels.Count == 0)
            {
                var data = ConstantMessages.AddIngredientFirstMsg;
                return RedirectToAction("AddIngredients", new {Data = data});
            }

            return this.View(bindingModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateRecipeViewModel bindingModel)
        {
            var username = this.User.Identity.Name;

            if (this.ModelState.IsValid)
            {
                this.recipeService.CreateRecipe(bindingModel, username);

                return RedirectToAction("All");
            }

            bindingModel =
                new CreateRecipeViewModel {IngredientsViewModels = this.recipeService.GetIngredients(username)};

            if (bindingModel.IngredientsViewModels == null || bindingModel.IngredientsViewModels.Count == 0)
            {
                var data = ConstantMessages.AddIngredientFirstMsg;
                return RedirectToAction("AddIngredients", new {Data = data});
            }

            return this.View(bindingModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddIngredients(string data, string searchString)
        {
            var username = this.User.Identity.Name;
            this.ViewData[GlobalConstants.ErrorString] = data;
            var products = this.productService.GetAll();
            var bindingModel = new CreateIngredientViewModel();

            if (!string.IsNullOrEmpty(searchString) && products != null)
            {
                this.SearchForGrocery(products, searchString, bindingModel);
            }
            else if(string.IsNullOrEmpty(searchString) && products != null)
            {
                bindingModel.Products = products.Select(product => this.mapper.Map<ProductViewModel>(product)).ToList();
            }

            bindingModel.Ingredients = this.recipeService.GetIngredients(username);

            return this.View(bindingModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddIngredients(CreateIngredientViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                var username = this.User.Identity.Name;
                this.recipeService.CreateIngredient(bindingModel, username);
                return RedirectToAction("AddIngredients");
            }

            var data = ConstantMessages.InvalidDataMsg;
            return RedirectToAction("AddIngredients", new {Data = data});
        }

        private void SearchForGrocery(IEnumerable<Product> products, string searchString,
            CreateIngredientViewModel model)
        {
            products = products.Where(p => (p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                            || p.ProductType.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)));

            var productsBindingModels = products.Select(product => this.mapper.Map<ProductViewModel>(product)).ToList();

            model.Products = productsBindingModels;
        }

        [Authorize]
        public IActionResult Recipe(string id)
        {
            if (this.recipeService.RecipeExists(id))
            {
                var recipe = this.recipeService.GetById(id);

                var recipeModel = this.MapSingleRecipe(recipe);

                return this.View(recipeModel);
            }

            var data = ConstantMessages.InvalidRecipeMsgError;

            return RedirectToAction("All", new {Data = data});
        }

        private RecipeViewModel MapSingleRecipe(Recipe recipe)
        {
            var recipeModel = this.mapper.Map<RecipeViewModel>(recipe);

            var base64 = Convert.ToBase64String(recipe.Image);
            var imgSrc = string.Format(ImageString, base64);
            recipeModel.PictureUrl = imgSrc;

            foreach (var ingredient in recipe.Ingredients)
            {
                recipeModel.IngredientsViewModel.Add(new IngredientsViewModel
                {
                    Name = ingredient.Name,
                    Quantity = ingredient.Quantity
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
