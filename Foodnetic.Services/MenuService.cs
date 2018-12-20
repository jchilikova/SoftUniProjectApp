using System;
using System.Collections.Generic;
using System.Linq;
using Foodnetic.Contants;
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

        public Menu GetDailyMenuForUser(string currentUser)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == currentUser)?.Id;

            return this.dbContext
                .Menus
                .Include(x => x.RecipeMenus)
                .ThenInclude(x => x.Recipe)
                .FirstOrDefault(x => x.UserId == userId && x.CreatedOn == DateTime.Today);
        }

        public ICollection<Menu> GetAllMenusForUser(string currentUser)
        {
            var userId = dbContext.Users.FirstOrDefault(x => x.UserName == currentUser)?.Id;

            return this.dbContext.Menus
                    .Include(x => x.RecipeMenus)
                .ThenInclude(x => x.Recipe)
                .Where(x => x.UserId == userId).ToList();
        }

        
        private RecipeMenu CreateBreakfast(IEnumerable<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == Constants.Strings.BreakfastString)?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);
               
                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu, Constants.Strings.BreakfastString);

                    DecreaseQuantityOfProducts(recipe, fridgeIngredients);
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateRecipeMenu(Recipe recipe, Menu menu, string menuType)
        {
            MenuType menuTypeEnum = (MenuType) 0;

            switch (menuType)
            {
                case Constants.Strings.BreakfastString:
                    menuTypeEnum = MenuType.Breakfast;
                    break;
                case Constants.Strings.LunchString:
                    menuTypeEnum = MenuType.Breakfast;
                    break;
                case Constants.Strings.DinnerString:
                    menuTypeEnum = MenuType.Dinner;
                    break;
                case Constants.Strings.DessertString:
                    menuTypeEnum = MenuType.Dessert;
                    break;
            }

            var menuBreakfast = new RecipeMenu
            {
                Recipe = recipe,
                Menu = menu,
                MenuType = menuTypeEnum
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
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == Constants.Strings.LunchString)?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);
               
                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu, Constants.Strings.LunchString);

                    DecreaseQuantityOfProducts(recipe, fridgeIngredients);
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateDinner(IEnumerable<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == Constants.Strings.DinnerString)?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);
               
                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu, Constants.Strings.DinnerString);

                    DecreaseQuantityOfProducts(recipe, fridgeIngredients);
                    return menuBreakfast;
                }
            }

            return null;
        }

        private RecipeMenu CreateDessert(IEnumerable<RecipeTag> recipes, List<Grocery> fridgeIngredients, Menu menu)
        {
            var breakfastTagId = dbContext.Tags.FirstOrDefault(x => x.Name.ToLower() == Constants.Strings.DessertString)?.Id;

            foreach (var recipeTag in recipes.Where(x => x.TagId == breakfastTagId))
            {
                var recipe = recipeTag.Recipe;
                var isRecipeValid = CheckIfUserHaveEnoughIngredients(recipe, fridgeIngredients);
               
                if (isRecipeValid)
                {
                    var menuBreakfast = CreateRecipeMenu(recipe, menu, Constants.Strings.DessertString);

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
