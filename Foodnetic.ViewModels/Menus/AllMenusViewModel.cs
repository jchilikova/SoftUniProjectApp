using System;

namespace Foodnetic.ViewModels.Menus
{
    public class AllMenusViewModel
    {
        public DateTime CreatedOn { get; set; }

        public string BreakfastRecipeId { get; set; }
        public string BreakfastRecipe { get; set; }
        
        public string LunchRecipeId { get; set; }
        public string LunchRecipe { get; set; }

        public string DinnerRecipeId { get; set; }
        public string DinnerRecipe { get; set; }

        public string DessertRecipeId { get; set; }
        public string DessertRecipe { get; set; }
    }
}
