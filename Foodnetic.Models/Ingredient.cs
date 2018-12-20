using System;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Ingredient : Product
    {
        public Ingredient()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int Quantity { get; set; }

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }
       
    }
}
