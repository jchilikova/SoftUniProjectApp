using System.Collections.Generic;
using Foodnetic.Models.Enums;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.ViewModels.Ingredients
{
    public class CreateIngredientViewModel
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public ICollection<IngredientsViewModel> Ingredients { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
