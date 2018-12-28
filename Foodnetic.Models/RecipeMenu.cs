using System.ComponentModel.DataAnnotations.Schema;
using Foodnetic.Models.Enums;

namespace Foodnetic.Models
{
    public class RecipeMenu
    {
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public string MenuId { get; set; }
        public Menu Menu { get; set; }

        [NotMapped]
        public DishType MenuType => this.Recipe.DishType;
    }
}
