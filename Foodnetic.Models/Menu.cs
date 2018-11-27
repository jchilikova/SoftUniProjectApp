using System;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Menu
    {
        public Menu()
        {
            this.Id = Guid.NewGuid().ToString();
            this.RecipeMenus = new HashSet<RecipeMenu>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        //public string BreakfastId { get; set; }
        //public Recipe Breakfast { get; set; }

        //public string LunchId { get; set; }
        //public Recipe Lunch { get; set; }

        //public string DinnerId { get; set; }
        //public Recipe Dinner { get; set; }

        //public string DessertId { get; set; }
        //public Recipe Dessert { get; set; }

        public ICollection<RecipeMenu> RecipeMenus { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
