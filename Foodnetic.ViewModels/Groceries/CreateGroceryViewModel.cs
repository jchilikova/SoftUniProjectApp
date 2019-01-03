using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.ViewModels.Groceries
{
    public class CreateGroceryViewModel
    {
        private const string RequiredError = " is required";
        private const int QuantityMinimumValue = 1;
        private const int QuantityMaximumValue = 10_000;

        [Required(ErrorMessage = nameof(ProductName) + RequiredError)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = nameof(Quantity) + RequiredError)]
        [Range(QuantityMinimumValue, QuantityMaximumValue, ErrorMessage = "Quantity must be 1 or more!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = nameof(ExpirationDate) + RequiredError)]
        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
