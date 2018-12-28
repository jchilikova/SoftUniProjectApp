using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models.Enums;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.ViewModels.Ingredients
{
    public class CreateIngredientViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Quantity is required!")]
        [Range(1, 2000, ErrorMessage = "Quantity must be in range 1 to 2000 grams")]
        public int Quantity { get; set; }

        public ICollection<IngredientsViewModel> Ingredients { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
