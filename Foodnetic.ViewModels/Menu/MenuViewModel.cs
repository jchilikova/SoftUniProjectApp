using System;
using System.Reflection.Metadata;
using Foodnetic.ViewModels.Recipes;

namespace Foodnetic.ViewModels.Menu
{
    public class MenuViewModel
    {
        public DateTime CreatedOn { get; set; }

        public AllRecipesViewModel Breakfast { get; set; }

        public AllRecipesViewModel Lunch { get; set; }

        public AllRecipesViewModel Dinner { get; set; }

        public AllRecipesViewModel Dessert { get; set; }
    }
}
