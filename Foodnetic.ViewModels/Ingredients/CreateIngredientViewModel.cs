using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models.Enums;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.ViewModels.Ingredients
{
    public class CreateIngredientViewModel
    {
        private const string RequiredError = " is required";
        private const int QuantityMinimumValue = 1;
        private const int QuantityMaximumValue = 10_000;

        [Required(ErrorMessage = nameof(Name) + RequiredError)]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(Quantity) + RequiredError)]
        [Range(QuantityMinimumValue, QuantityMaximumValue, ErrorMessage = "Quantity must be 1 or more!")]
        public int Quantity { get; set; }

        public ICollection<IngredientsViewModel> Ingredients { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
