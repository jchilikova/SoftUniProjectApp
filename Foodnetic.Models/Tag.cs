using System;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Recipes = new HashSet<RecipeTag>();
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<RecipeTag> Recipes { get; set; }    
    }
}
