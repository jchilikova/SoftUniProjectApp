using System;
using System.Collections.Generic;
using System.Linq;
using Foodnetic.Constants;
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

        public MenuService(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool CheckIfMenuExist(string username)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == username)?.Id;
            var menu = this.dbContext.Menus.FirstOrDefault(x => x.UserId == userId && x.CreatedOn == DateTime.Today);

            return menu != null;
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
                    .Include(x => x.Recipe.Stars)
                .ToList();

            //creating menu
            var menu = new Menu
            {
                Name = $"{DateTime.Now.Date}_{username}",
                UserId = userId,
                CreatedOn = DateTime.Today.Date
            };

            this.dbContext.Menus.Add(menu);

            if (fridgeIngredients == null)
            {
                return menu;
            }

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

        public Menu GetDailyMenuForUser(string currentUser)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == currentUser)?.Id;

            return this.dbContext
                .Menus
                .Include(x => x.RecipeMenus)
                .ThenInclude(x => x.Recipe)
                .ThenInclude(x => x.Stars)
                .FirstOrDefault(x => x.UserId == userId && x.CreatedOn == DateTime.Today);
        }

        public ICollection<Menu> GetAllMenusForUser(string currentUser)
        {
            FoodneticUser user = (FoodneticUser)dbContext
                .Users
                .FirstOrDefault(x => x.UserName == currentUser);

            var dailyMenus = this.dbContext.Menus.Any(x => x.UserId == user.Id);

            if (dailyMenus)
            {
                return this.dbContext.Menus
                    .Include(x => x.RecipeMenus)
                    .ThenInclude(x => x.Recipe)
                    .Where(x => x.UserId == user.Id).ToList();
            }

            return null;

        }

        private RecipeMenu CreateBreakfast(IEnumerable<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == GlobalConstants.BreakfastString)?.Id;
            var recipesOrdered = recipes
                .OrderByDescending(x => x.Recipe.Rating)
                .Where(x => x.TagId == breakfastTagId || x.Recipe.DishType == DishType.Breakfast);
               

            foreach (var recipeTag in recipesOrdered)
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);

                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu);

                    DecreaseQuantityOfProducts(recipe, fridgeIngredients);
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateRecipeMenu(Recipe recipe, Menu menu)
        {
            var menuBreakfast = new RecipeMenu
            {
                Recipe = recipe,
                Menu = menu,
            };

            return menuBreakfast;
        }

        private bool CheckIfUserHaveEnoughIngredients(Recipe recipe, IEnumerable<Grocery> fridgeIngredients)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                var grocery = fridgeIngredients.FirstOrDefault(x => x.Name == ingredient.Name);

                if (grocery == null || grocery.Quantity < ingredient.Quantity)
                {
                    return false;
                }
            }

            return true;

        }

        private RecipeMenu CreateLunch(IEnumerable<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == GlobalConstants.LunchString)?.Id;
            var recipesOrdered = recipes
                .OrderByDescending(x => x.Recipe.Rating)
                .Where(x => x.TagId == breakfastTagId || x.Recipe.DishType == DishType.Lunch);

            foreach (var recipeTag in recipesOrdered)
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);

                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu);

                    DecreaseQuantityOfProducts(recipe, fridgeIngredients);
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateDinner(IEnumerable<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == GlobalConstants.DinnerString)?.Id;
            var recipesOrdered = recipes
                .OrderByDescending(x => x.Recipe.Rating)
                .Where(x => x.TagId == breakfastTagId || x.Recipe.DishType == DishType.Dinner);

            foreach (var recipeTag in recipesOrdered)
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);

                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu);

                    DecreaseQuantityOfProducts(recipe, fridgeIngredients);
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateDessert(IEnumerable<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == GlobalConstants.DessertString)?.Id;
            var recipesOrdered = recipes
                .OrderByDescending(x => x.Recipe.Rating)
                .Where(x => x.TagId == breakfastTagId || x.Recipe.DishType == DishType.Dessert);

            foreach (var recipeTag in recipesOrdered)
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);

                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu);

                    DecreaseQuantityOfProducts(recipe, fridgeIngredients);
                    return menuBreakfast;
                }
            }

            return null;
        }

        private void DecreaseQuantityOfProducts(Recipe recipe, IEnumerable<Grocery> fridgeIngredients)
        {
            var groceriesOutOfStock = new List<Grocery>();

            foreach (var ingredient in recipe.Ingredients)
            {
                var grocery = fridgeIngredients.FirstOrDefault(x => x.Name == ingredient.Name);

                grocery.Quantity -= ingredient.Quantity;

                if (grocery.Quantity <= 0)
                {
                    groceriesOutOfStock.Add(grocery);
                }
            }

            RemoveOutOfStockProducts(groceriesOutOfStock);
        }

        private void RemoveOutOfStockProducts(IEnumerable<Grocery> groceriesOutOfStock)
        {
            this.dbContext.Groceries.RemoveRange(groceriesOutOfStock);
            this.dbContext.SaveChanges();
        }
    }
}
