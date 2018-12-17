using System;

namespace Foodnetic.ViewModels.Menu
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
