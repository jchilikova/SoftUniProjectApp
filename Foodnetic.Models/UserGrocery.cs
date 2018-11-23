using System;

namespace Foodnetic.Models
{
    public class UserGrocery
    {
        public UserGrocery()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string GroceryId { get; set; }
        public Grocery Grocery { get; set; }
    }
}
