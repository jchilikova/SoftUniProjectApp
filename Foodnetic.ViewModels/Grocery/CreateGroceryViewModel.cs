using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models;

namespace Foodnetic.ViewModels.Grocery
{
    public class CreateGroceryViewModel
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
