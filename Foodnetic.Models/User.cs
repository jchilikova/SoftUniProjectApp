using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Comments = new HashSet<Comment>();
        }

        public string FirstName { get; set; }

        public string LastName  { get; set; }

        public string DailyMenuId { get; set; }
        public Menu DailyMenu { get; set; }

        public string VirtualFridgeId { get; set; }
        public VirtualFridge VirtualFridge { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
