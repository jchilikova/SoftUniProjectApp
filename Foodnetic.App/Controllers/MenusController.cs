using System;
using System.Globalization;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Menu;
using Foodnetic.ViewModels.Recipes;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class MenusController : Controller
    {
        private readonly IMenuService menuService;

        public MenusController(IMenuService menuService)
        {
            this.menuService = menuService;
        }
        public IActionResult Index()
        {
            var currentUser = this.User.Identity.Name;

            if (menuService.CheckIfMenuExist(currentUser))
            {
                var menu = this.menuService.GetMenu(currentUser);

                var bindingModel = new MenuViewModel { CreatedOn = menu.CreatedOn.Date };

                foreach (var recipeMenu in menu.RecipeMenus)
                {
                    if (recipeMenu != null && recipeMenu.MenuType == MenuType.Breakfast)
                    {
                        bindingModel.Breakfast = new AllRecipesViewModel
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
                        bindingModel.Lunch = new AllRecipesViewModel
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
                        bindingModel.Dinner = new AllRecipesViewModel
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
                        bindingModel.Dessert = new AllRecipesViewModel
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

            return RedirectToAction("Index");
        }
    }
}