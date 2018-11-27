using System;

namespace Foodnetic.Models
{
    public class RecipeIngredient
    {
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
