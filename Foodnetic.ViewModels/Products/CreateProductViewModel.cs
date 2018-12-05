using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models.Enums;

namespace Foodnetic.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Product type")]
        public ProductType ProductType { get; set; }
    }
}
