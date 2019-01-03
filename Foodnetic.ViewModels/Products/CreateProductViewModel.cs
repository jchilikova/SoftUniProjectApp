using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models.Enums;

namespace Foodnetic.ViewModels.Products
{
    public class CreateProductViewModel
    {
        private const string RequiredError = " is required";

        [Required(ErrorMessage = nameof(Name) + RequiredError)]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(ProductType) + RequiredError)]
        [DisplayName("Product type")]
        public ProductType ProductType { get; set; }
    }
}
