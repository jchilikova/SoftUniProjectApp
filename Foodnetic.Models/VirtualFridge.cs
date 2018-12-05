using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class VirtualFridge
    {
        public VirtualFridge()
        {
            this.Groceries = new HashSet<Grocery>();
        }

        public string Id { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<Grocery> Groceries { get; set; }
    }
}
