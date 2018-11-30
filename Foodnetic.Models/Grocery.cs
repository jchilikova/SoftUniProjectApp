using System;
using System.Collections;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Grocery : Ingredient
    {
        public Grocery()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime ExpirationDate { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
