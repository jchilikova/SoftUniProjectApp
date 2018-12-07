﻿using System.Collections.Generic;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Recipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodnetic.App.Controllers
{
    public class RecipesController : Controller
    {
        private readonly FoodneticDbContext dbContext;
        private readonly IRecipeService recipeService;

        public RecipesController(FoodneticDbContext dbContext, IRecipeService recipeService)
        {
            this.dbContext = dbContext;
            this.recipeService = recipeService;
        }

        public IActionResult All()
        {
            var recipesModels = this.recipeService.GetAll();

            return this.View(recipesModels);
        }

        public IActionResult Create()
        {
            //var recipe1 = new Recipe
            //{
            //    Name = "Kiwi and banana smoothie bowl",
            //    Description = "Kiwi and banana smoothie ball with chia seed, perfect for your breakfast.",
            //    AuthorId =  "6d62c902-ad5c-4bea-aca1-33b70f34697e",
            //    Rating = 4,
            //    CookTime = 2,
            //    Directions = "First you chop the banana and the kiwi, add to the chopper and then eat. Add chia seeds and maple syrup",
            //    PreparationTime = 10,
            //    PictureUrl = @"~/images/recipes/kiwiBreakfast.jpg"
            //};

            //var recipe2 = new Recipe
            //{
            //    Name = "Standart Pancakes",
            //    Description = "Pancakes for breakfast.",
            //    AuthorId =  "6d62c902-ad5c-4bea-aca1-33b70f34697e",
            //    Rating = 1.4m,
            //    CookTime = 20,
            //    Directions = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            //    PreparationTime = 10,
            //    PictureUrl = @"~/images/recipes/pancakes.jpg"
            //};

            //var recipe3 = new Recipe
            //{
            //    Name = "Spinach, avocado and eggs",
            //    Description = "Perfect healthy lunch idea!",
            //    AuthorId =  "6d62c902-ad5c-4bea-aca1-33b70f34697e",
            //    Rating = 2,
            //    CookTime = 5,
            //    Directions = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            //    PreparationTime = 15,
            //    PictureUrl = @"~/images/recipes/avocadoAndEggsLunch.jpg"
            //};

            //var recipe4 = new Recipe
            //{
            //    Name = "Cake with frosting",
            //    Description = "Dessert for everyone",
            //    AuthorId =  "6d62c902-ad5c-4bea-aca1-33b70f34697e",
            //    Rating = 5,
            //    CookTime = 30,
            //    Directions = "First you chop the banana and the kiwi, add to the chopper and then eat. Add chia seeds and maple syrup",
            //    PreparationTime = 55,
            //    PictureUrl = @"~/images/recipes/whiteCake.jpg"
            //};

            //var recipe5 = new Recipe
            //{
            //    Name = "Ham sandwich",
            //    Description = "Ham sandwich with veggies for perfect lunch!",
            //    AuthorId =  "6d62c902-ad5c-4bea-aca1-33b70f34697e",
            //    Rating = 4.5m,
            //    CookTime = 2,
            //    Directions = "Add ham and eat Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            //    PreparationTime = 30,
            //    PictureUrl = @"~/images/recipes/sandwich.jpg"
            //};

            //var recipe6 = new Recipe
            //{
            //    Name = "Fluffy apple muffins",
            //    Description = "Apple muffins for perfect dessert for you and your family",
            //    AuthorId =  "6d62c902-ad5c-4bea-aca1-33b70f34697e",
            //    Rating = 3,
            //    CookTime = 30,
            //    Directions = "First you chop the banana and the kiwi, add to the chopper and then eat. Add chia seeds and maple syrup",
            //    PreparationTime = 20,
            //    PictureUrl = @"~/images/recipes/appleMuffins.jpg"
            //};

            //var recipes = new List<Recipe> {recipe1, recipe2, recipe3, recipe4, recipe5, recipe6};

            //this.dbContext.Recipes.AddRange(recipes);
            //this.dbContext.SaveChanges();

            return this.View();

        }

        //[HttpPost]
        //public IActionResult Create()
        //{
        //    return this.View();
        //}


        public IActionResult Recipe(string id)
        {
            var recipe = this.recipeService.GetById(id);

            return this.View(recipe);
        }
    }
}
