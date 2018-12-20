using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models.Enums;

namespace Foodnetic.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Name of product is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product type is required!")]
        [DisplayName("Product type")]
        public ProductType ProductType { get; set; }
    }
}
