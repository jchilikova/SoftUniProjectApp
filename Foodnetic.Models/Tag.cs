using System;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Recipes = new HashSet<Recipe>();
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }    
    }
}
