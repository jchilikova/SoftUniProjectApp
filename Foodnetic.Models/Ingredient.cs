using System;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Ingredient : Product
    {
        public Ingredient()
        {
            this.Recipes = new HashSet<Recipe>();
            this.Id = Guid.NewGuid().ToString();
        }

        public int Quantity { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
       
    }
}
