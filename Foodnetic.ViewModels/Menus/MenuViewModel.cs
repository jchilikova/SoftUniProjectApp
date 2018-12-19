using System;
using Foodnetic.ViewModels.Menu;

namespace Foodnetic.ViewModels.Menus
{
    public class MenuViewModel
    {
        public DateTime CreatedOn { get; set; }

        public MenuRecipeViewModel Breakfast { get; set; }

        public MenuRecipeViewModel Lunch { get; set; }

        public MenuRecipeViewModel Dinner { get; set; }

        public MenuRecipeViewModel Dessert { get; set; }
    }
}
