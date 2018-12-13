using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Foodnetic.Contants;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Menu;
using Foodnetic.ViewModels.Recipes;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Foodnetic.App.Controllers
{
    public class MenusController : Controller
    {
        private readonly IMenuService menuService;
        private readonly IMapper mapper;

        public MenusController(IMenuService menuService, IMapper mapper)
        {
            this.menuService = menuService;
            this.mapper = mapper;
        }

        public IActionResult Index(string data)
        {
            this.ViewData["Error"] = data;
            var currentUser = this.User.Identity.Name;

            if (menuService.CheckIfMenuExist(currentUser))
            {
                var menu = this.menuService.GetDailyMenuForUser(currentUser);

                var bindingModel = new MenuViewModel { CreatedOn = menu.CreatedOn.Date };

                foreach (var recipeMenu in menu.RecipeMenus)
                {
                    if (recipeMenu != null && recipeMenu.MenuType == MenuType.Breakfast)
                    {
                        bindingModel.Breakfast = new MenuRecipeViewModel
                        {
                            Description = recipeMenu.Recipe.Description,
                            Id = recipeMenu.RecipeId,
                            Name = recipeMenu.Recipe.Name,
                            PictureUrl = recipeMenu.Recipe.PictureUrl,
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
                            PictureUrl = recipeMenu.Recipe.PictureUrl,
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
                            PictureUrl = recipeMenu.Recipe.PictureUrl,
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
                            PictureUrl = recipeMenu.Recipe.PictureUrl,
                            Rating = recipeMenu.Recipe.Rating
                        };
                        continue;
                    }
                    else
                    {
                        bindingModel.Dessert = null;
                    }
                }

                return this.View(bindingModel);
            }
            else
            {
                return View(null);
            }

        }

        public IActionResult Create()
        {
            var currentUser = this.User.Identity.Name;

            var menu = this.menuService.Create(currentUser);
            string data = null;

            if (menu.RecipeMenus.Count <= 0)
            {
                data =
                   Constants.Messages.NotEnoughGroceriesError;
            }

            return RedirectToAction("Index",  new { Data = data });
        }

        public IActionResult History(int? page)
        {
            var currentUser = this.User.Identity.Name;

            var menus = this.menuService.GetAllMenusForUser(currentUser);

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

            var nextPage = page ?? 1;

            var pageViewModel = menusBindingModels.ToPagedList(nextPage, 5);

            return this.View(pageViewModel);
        }
    }
}