using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models;

namespace Foodnetic.ViewModels.Grocery
{
    public class CreateGroceryViewModel
    {
        public string Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
