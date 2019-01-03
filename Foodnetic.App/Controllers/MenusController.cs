using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Constants;
using Foodnetic.Infrastructure;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Foodnetic.App.Controllers
{
    public class MenusController : Controller
    {
        private const string Dash = "-";
        private readonly IMenuService menuService;

        public MenusController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [Authorize]
        public IActionResult Index(string data)
        {
            this.ViewData[GlobalConstants.ErrorString] = data;
            var currentUser = this.User.Identity.Name;

            if (menuService.CheckIfMenuExist(currentUser))
            {
                var menu = this.menuService.GetDailyMenuForUser(currentUser);

                var bindingModel = this.MapMenuModel(menu);

                return this.View(bindingModel);
            }
            else
            {
                return View(null);
            }

        }

        [Authorize]
        public IActionResult Create()
        {
            var currentUser = this.User.Identity.Name;

            var menu = this.menuService.Create(currentUser);
            string data = null;

            if (menu.RecipeMenus.Count <= 0)
            {
                data =
                   ConstantMessages.NotEnoughGroceriesErrorMsg;
            }

            return RedirectToAction("Index", new { Data = data });
        }

        [Authorize]
        public IActionResult History(int? page)
        {
            var currentUser = this.User.Identity.Name;

            var menus = this.menuService.GetAllMenusForUser(currentUser);

            if (menus == null)
            {
                return this.View();
            }

            var orderByDescendingMenus = menus.OrderByDescending(x => x.CreatedOn);

            var menusBindingModels = this.MapAllMenus(orderByDescendingMenus);

            var nextPage = page ?? 1;

            var pageViewModel = menusBindingModels.ToPagedList(nextPage, 5);

            return this.View(pageViewModel);
        }

        private IEnumerable<AllMenusViewModel> MapAllMenus(IEnumerable<Menu> menus)
        {
            var menusBindingModels = new List<AllMenusViewModel>();

            foreach (var menu in menus)
            {
                var bindingModel = new AllMenusViewModel
                {
                    CreatedOn = menu.CreatedOn,
                    BreakfastRecipe = Dash,
                    DessertRecipe = Dash,
                    DinnerRecipe = Dash,
                    LunchRecipe = Dash
                };

                if (menu.RecipeMenus.Any(x => x.MenuType == DishType.Breakfast))
                {
                    var recipe = menu.RecipeMenus.FirstOrDefault(x => x.MenuType == DishType.Breakfast)?.Recipe;
                    bindingModel.BreakfastRecipe = recipe.Name;
                    bindingModel.BreakfastRecipeId = recipe.Id;
                }

                if (menu.RecipeMenus.Any(x => x.MenuType == DishType.Lunch))
                {
                    var recipe = menu.RecipeMenus.FirstOrDefault(x => x.MenuType == DishType.Lunch)?.Recipe;
                    bindingModel.LunchRecipe = recipe.Name;
                    bindingModel.LunchRecipeId = recipe.Id;
                }

                if (menu.RecipeMenus.Any(x => x.MenuType == DishType.Dinner))
                {
                    var recipe = menu.RecipeMenus.FirstOrDefault(x => x.MenuType == DishType.Dinner)?.Recipe;
                    bindingModel.DinnerRecipe = recipe.Name;
                    bindingModel.DinnerRecipeId = recipe.Id;
                }

                if (menu.RecipeMenus.Any(x => x.MenuType == DishType.Dessert))
                {
                    var recipe = menu.RecipeMenus.FirstOrDefault(x => x.MenuType == DishType.Dessert)?.Recipe;
                    bindingModel.DessertRecipe = recipe.Name;
                    bindingModel.DessertRecipeId = recipe.Id;
                }

                menusBindingModels.Add(bindingModel);
            }

            return menusBindingModels;

        }

        private MenuViewModel MapMenuModel(Menu menu)
        {
            var bindingModel = new MenuViewModel { CreatedOn = menu.CreatedOn.Date };

            foreach (var recipeMenu in menu.RecipeMenus)
            {
                var base64 = Convert.ToBase64String(recipeMenu.Recipe.Image);
                var imgSrc = $"data:image/jpg;base64,{base64}";

                if (recipeMenu.MenuType == DishType.Breakfast)
                {
                    bindingModel.Breakfast = new MenuRecipeViewModel
                    {
                        Description = recipeMenu.Recipe.Description,
                        Id = recipeMenu.RecipeId,
                        Name = recipeMenu.Recipe.Name,
                        PictureUrl = imgSrc,
                        Rating = recipeMenu.Recipe.Rating
                    };

                    continue;
                }
                if (recipeMenu.MenuType == DishType.Lunch)
                {
                    bindingModel.Lunch = new MenuRecipeViewModel
                    {
                        Description = recipeMenu.Recipe.Description,
                        Id = recipeMenu.RecipeId,
                        Name = recipeMenu.Recipe.Name,
                        PictureUrl = imgSrc,
                        Rating = recipeMenu.Recipe.Rating
                    };
                    continue;
                }
               if (recipeMenu.MenuType == DishType.Dinner)
                {
                    bindingModel.Dinner = new MenuRecipeViewModel
                    {
                        Description = recipeMenu.Recipe.Description,
                        Id = recipeMenu.RecipeId,
                        Name = recipeMenu.Recipe.Name,
                        PictureUrl = imgSrc,
                        Rating = recipeMenu.Recipe.Rating
                    };
                    continue;
                }
                if (recipeMenu.MenuType == DishType.Dessert)
                {
                    bindingModel.Dessert = new MenuRecipeViewModel
                    {
                        Description = recipeMenu.Recipe.Description,
                        Id = recipeMenu.RecipeId,
                        Name = recipeMenu.Recipe.Name,
                        PictureUrl = imgSrc,
                        Rating = recipeMenu.Recipe.Rating
                    };
                    continue;
                }
            }

            return bindingModel;
        }
    }
}