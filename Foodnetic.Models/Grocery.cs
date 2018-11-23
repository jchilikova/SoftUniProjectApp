using System;
using System.Collections;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Grocery : Ingredient
    {
        public Grocery()
        {
            this.Users = new HashSet<User>();
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime ExpirationDate { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
