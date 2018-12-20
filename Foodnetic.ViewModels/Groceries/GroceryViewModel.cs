using System;

namespace Foodnetic.ViewModels.Groceries
{
    public class GroceryViewModel
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
