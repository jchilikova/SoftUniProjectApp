using System;

namespace Foodnetic.Models
{
    public class Grocery : Ingredient
    {
        public Grocery()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime ExpirationDate { get; set; }

        public string VirtualFridgeId { get; set; }
        public VirtualFridge VirtualFridge { get; set; }
    }
}
