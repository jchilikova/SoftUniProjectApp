using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Foodnetic.Contants;
using Foodnetic.Data;
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

namespace Foodnetic.App.Controllers
{
    public class RecipesController : Controller
    {
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
            CreateRecipeViewModel bindingModel =
                new CreateRecipeViewModel {IngredientsViewModels = this.recipeService.GetIngredients()};

            return this.View(bindingModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateRecipeViewModel bindingModel)
        {
            var username = this.User.Identity.Name;
            this.recipeService.CreateRecipe(bindingModel, username);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddIngredients(string searchString)
        {
            var products = this.productService.GetAll();

            if (string.IsNullOrEmpty(searchString)) return this.View();

            var bindingModel = this.SearchForGrocery(products, searchString);
            bindingModel.Ingredients = this.recipeService.GetIngredients();

            return this.View(bindingModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddIngredients(CreateIngredientViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                this.recipeService.CreateIngredient(bindingModel);
                return this.View();
            }

            return RedirectToAction("AddIngredients");
        }

        private CreateIngredientViewModel SearchForGrocery(IEnumerable<Product> products, string searchString)
        {
            products = products.Where(p => (p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                            || p.ProductType.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)));

            var productsBindingModels = products.Select(product => this.mapper.Map<ProductViewModel>(product)).ToList();

            var bindingModel = new CreateIngredientViewModel
            {
                Products = productsBindingModels
            };

            return bindingModel;
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

            var data = Constants.Messages.InvalidRecipeMsgError;

            return RedirectToAction("All", new {Data = data});
        }

        //[HttpPost]  
        //public JsonResult GetProductType()  
        //{  
        //    var productTypes = new List<string>();

        //    foreach (ProductType type in (ProductType[]) Enum.GetValues(typeof(ProductType)))
        //    {
        //        productTypes.Add(type.ToString());
        //    }

        //    return Json(productTypes);  
        //}   

        //[HttpPost]  
        //public JsonResult GetProduct(string productType)  
        //{  
        //    var products = new List<string>();
        //    var type = (ProductType)Enum.Parse(typeof(ProductType), productType);

        //    if (!string.IsNullOrWhiteSpace(productType))
        //    {
        //        products = this.dbContext.Products.Where(x => x.ProductType == type && x.GetType() == typeof(Product)).Select(x => x.Name).ToList();
        //    }  

        //    return Json(products);  
        //}   

        private RecipeViewModel MapSingleRecipe(Recipe recipe)
        {
            var recipeModel = this.mapper.Map<RecipeViewModel>(recipe);

            var base64 = Convert.ToBase64String(recipe.Image);
            var imgSrc = $"data:image/jpg;base64,{base64}";
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
