using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.Models
{
    public class FoodneticUser : IdentityUser
    {
        public FoodneticUser()
        {
            this.Comments = new HashSet<Comment>();
            this.DailyMenus = new HashSet<Menu>();
        }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string LastName  { get; set; }

        public ICollection<Menu> DailyMenus { get; set; }

        public string VirtualFridgeId { get; set; }
        public VirtualFridge VirtualFridge { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
