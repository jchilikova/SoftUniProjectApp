using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models;

namespace Foodnetic.ViewModels.Groceries
{
    public class CreateGroceryViewModel
    {
        [Required(ErrorMessage = "Product is required!")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be 1 or more!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Expiration Date is required!")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
