using System;

namespace Foodnetic.Models
{
    public class RecipeTag
    {
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
