using System;
using System.Collections.Generic;
using System.Linq;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Foodnetic.Services
{
    public class MenuService : IMenuService
    {
        private readonly FoodneticDbContext dbContext;

        public bool CheckIfMenuExist(string username)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == username)?.Id;
            var menu = this.dbContext.Menus.FirstOrDefault(x => x.UserId == userId);

            if (menu != null && menu.CreatedOn == DateTime.Today)
            {
                return true;
            }

            return false;
        }

        public MenuService(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Menu Create(string username)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == username)?.Id;

            //user's groceries
            var fridgeIngredients = this.dbContext.
                VirtualFridges
                .Include(f => f.Groceries)
                .FirstOrDefault(x => x.OwnerId == userId)
                ?.Groceries
                .Where(x => x.IsExpired == false)
                .ToList();

            //all recipes
            var recipes = dbContext.RecipeTags
                    .Include(x => x.Recipe)
                .ThenInclude(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .ToList();

            //creating menu
            var menu = new Menu
            {
                Name = $"{DateTime.Now.Date}_{username}",
                UserId = userId,
                CreatedOn = DateTime.Today.Date
            };

            this.dbContext.Menus.Add(menu);

            //creating breakfast
            var breakfastMenu = CreateBreakfast(recipes, fridgeIngredients, menu);

            if (breakfastMenu != null)
            {
                this.dbContext.RecipeMenus.Add(breakfastMenu);
                menu.RecipeMenus.Add(breakfastMenu);
                this.dbContext.SaveChanges();
            }

            //creating lunch
            var lunchMenu = CreateLunch(recipes, fridgeIngredients, menu);

            if (lunchMenu != null)
            {
                this.dbContext.RecipeMenus.Add(lunchMenu);
                menu.RecipeMenus.Add(lunchMenu);
                this.dbContext.SaveChanges();
            }

            //creating dinner
            var dinnerMenu = CreateDinner(recipes, fridgeIngredients, menu);

            if (dinnerMenu != null)
            {
                this.dbContext.RecipeMenus.Add(dinnerMenu);
                menu.RecipeMenus.Add(dinnerMenu);
                this.dbContext.SaveChanges();
            }

            //creating dessert
            var dessertMenu = CreateDessert(recipes, fridgeIngredients, menu);

            if (dessertMenu != null)
            {
                this.dbContext.RecipeMenus.Add(dessertMenu);
                menu.RecipeMenus.Add(dessertMenu);
                this.dbContext.SaveChanges();
            }


            return menu;

        }

        private RecipeMenu CreateBreakfast(List<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var groceriesOutOfStock = new List<Grocery>();
            var menuBreakfast = new RecipeMenu();
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == "breakfast")?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = true;

                foreach (var ingredient in recipe.Ingredients)
                {
                    if (!isRecipeValid)
                    {
                        continue;
                    }
                    var grocery = fridgeIngredients.FirstOrDefault(x => x.Name == ingredient.Ingredient.Name);

                    if (grocery != null && grocery.Quantity >= ingredient.Ingredient.Quantity)
                    {
                        grocery.Quantity -= ingredient.Ingredient.Quantity;

                        if (grocery.Quantity <= 10)
                        {
                            groceriesOutOfStock.Add(grocery);
                        }
                    }
                    else
                    {
                        isRecipeValid = false;
                    }
                }

                if (isRecipeValid)
                {
                    menuBreakfast.Recipe = recipe;
                    menuBreakfast.Menu = menu;
                    menuBreakfast.MenuType = MenuType.Breakfast;

                    this.dbContext.Groceries.RemoveRange(groceriesOutOfStock);
                    this.dbContext.SaveChanges();
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateLunch(List<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var groceriesOutOfStock = new List<Grocery>();
            var menuBreakfast = new RecipeMenu();
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == "lunch")?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = true;

                foreach (var ingredient in recipe.Ingredients)
                {
                    if (!isRecipeValid)
                    {
                        continue;
                    }
                    var grocery = fridgeIngredients.FirstOrDefault(x => x.Name == ingredient.Ingredient.Name);

                    if (grocery != null && grocery.Quantity >= ingredient.Ingredient.Quantity)
                    {
                        grocery.Quantity -= ingredient.Ingredient.Quantity;

                        if (grocery.Quantity <= 10)
                        {
                            groceriesOutOfStock.Add(grocery);
                        }
                    }
                    else
                    {
                        isRecipeValid = false;
                    }
                }

                if (isRecipeValid)
                {
                    menuBreakfast.Recipe = recipe;
                    menuBreakfast.Menu = menu;
                    menuBreakfast.MenuType = MenuType.Lunch;


                    this.dbContext.Groceries.RemoveRange(groceriesOutOfStock);
                    this.dbContext.SaveChanges();
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateDinner(List<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var groceriesOutOfStock = new List<Grocery>();
            var menuBreakfast = new RecipeMenu();
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == "dinner")?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = true;

                foreach (var ingredient in recipe.Ingredients)
                {
                    if (!isRecipeValid)
                    {
                        continue;
                    }
                    var grocery = fridgeIngredients.FirstOrDefault(x => x.Name == ingredient.Ingredient.Name);

                    if (grocery != null && grocery.Quantity >= ingredient.Ingredient.Quantity)
                    {
                        grocery.Quantity -= ingredient.Ingredient.Quantity;

                        if (grocery.Quantity <= 10)
                        {
                            groceriesOutOfStock.Add(grocery);
                        }
                    }
                    else
                    {
                        isRecipeValid = false;
                       
                    }
                }

                if (isRecipeValid)
                {
                    menuBreakfast.Recipe = recipe;
                    menuBreakfast.Menu = menu;
                    menuBreakfast.MenuType = MenuType.Dinner;


                    this.dbContext.Groceries.RemoveRange(groceriesOutOfStock);
                    this.dbContext.SaveChanges();
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateDessert(List<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var groceriesOutOfStock = new List<Grocery>();
            var menuBreakfast = new RecipeMenu();
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == "dessert")?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = true;

                foreach (var ingredient in recipe.Ingredients)
                {
                    if (!isRecipeValid)
                    {
                        continue;
                    }

                    var grocery = fridgeIngredients.FirstOrDefault(x => x.Name == ingredient.Ingredient.Name);

                    if (grocery != null && grocery.Quantity >= ingredient.Ingredient.Quantity)
                    {
                        grocery.Quantity -= ingredient.Ingredient.Quantity;

                        if (grocery.Quantity <= 10)
                        {
                            groceriesOutOfStock.Add(grocery);
                        }
                    }
                    else
                    {
                        isRecipeValid = false;
                    }
                }

                if (isRecipeValid)
                {
                    menuBreakfast.Recipe = recipe;
                    menuBreakfast.Menu = menu;
                    menuBreakfast.MenuType = MenuType.Dessert;


                    this.dbContext.Groceries.RemoveRange(groceriesOutOfStock);
                    this.dbContext.SaveChanges();
                    return menuBreakfast;
                }
            }

            return null;
        }

        public Menu GetMenu(string currentUser)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == currentUser)?.Id;

            return this.dbContext.Menus.Include(x => x.RecipeMenus).ThenInclude(x => x.Recipe).FirstOrDefault(x => x.UserId == userId);
        }
    }
}
