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

        public ICollection<RecipeMenu> RecipeMenus { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }
        public FoodneticUser User { get; set; }
    }
}
