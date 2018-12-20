using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Contants;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Menu;
using Foodnetic.ViewModels.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Foodnetic.App.Controllers
{
    public class MenusController : Controller
    {
        private readonly IMenuService menuService;

        public MenusController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [Authorize]
        public IActionResult Index(string data)
        {
            this.ViewData[Constants.Strings.ErrorString] = data;
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
                   Constants.Messages.NotEnoughGroceriesErrorMsg;
            }

            return RedirectToAction("Index", new { Data = data });
        }

        [Authorize]
        public IActionResult History(int? page)
        {
            var currentUser = this.User.Identity.Name;

            var menus = this.menuService.GetAllMenusForUser(currentUser).OrderByDescending(x => x.CreatedOn);

            var menusBindingModels = this.MapAllMenus(menus);

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
                    BreakfastRecipe = "-",
                    DessertRecipe = "-",
                    DinnerRecipe = "-",
                    LunchRecipe = "-"
                };

                if (menu.RecipeMenus.Any(x => x.MenuType == MenuType.Breakfast))
                {
                    bindingModel.BreakfastRecipe =
                        menu.RecipeMenus.FirstOrDefault(x => x.MenuType == MenuType.Breakfast)?.Recipe.Name;
                }

                if (menu.RecipeMenus.Any(x => x.MenuType == MenuType.Lunch))
                {
                    bindingModel.LunchRecipe =
                        menu.RecipeMenus.FirstOrDefault(x => x.MenuType == MenuType.Lunch)?.Recipe.Name;
                }

                if (menu.RecipeMenus.Any(x => x.MenuType == MenuType.Dinner))
                {
                    bindingModel.DinnerRecipe =
                        menu.RecipeMenus.FirstOrDefault(x => x.MenuType == MenuType.Dinner)?.Recipe.Name;
                }

                if (menu.RecipeMenus.Any(x => x.MenuType == MenuType.Dessert))
                {
                    bindingModel.DessertRecipe =
                        menu.RecipeMenus.FirstOrDefault(x => x.MenuType == MenuType.Dessert)?.Recipe.Name;
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

                if (recipeMenu != null && recipeMenu.MenuType == MenuType.Breakfast)
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
                else
                {
                    bindingModel.Breakfast = null;
                }

                if (recipeMenu != null && recipeMenu.MenuType == MenuType.Lunch)
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
                else
                {
                    bindingModel.Lunch = null;
                }

                if (recipeMenu != null && recipeMenu.MenuType == MenuType.Dinner)
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
                else
                {
                    bindingModel.Dinner = null;
                }

                if (recipeMenu != null && recipeMenu.MenuType == MenuType.Dessert)
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
                else
                {
                    bindingModel.Dessert = null;
                }
            }

            return bindingModel;
        }
    }
}