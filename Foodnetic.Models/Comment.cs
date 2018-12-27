using System;

namespace Foodnetic.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string AuthorId { get; set; }
        public FoodneticUser Author { get; set; }

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
