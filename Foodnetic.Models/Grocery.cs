using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodnetic.Models
{
    public class Grocery : Ingredient
    {
        public Grocery()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime ExpirationDate { get; set; }

        [NotMapped]
        public bool IsExpired => ExpirationDate <= DateTime.Today;

        public string VirtualFridgeId { get; set; }
        public VirtualFridge VirtualFridge { get; set; }
    }
}
