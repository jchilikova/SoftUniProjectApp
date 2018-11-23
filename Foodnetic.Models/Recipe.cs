using System;
using System.Collections.Generic;

namespace Foodnetic.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Ingredients = new HashSet<Ingredient>();
            this.Comment = new HashSet<Comment>();
            this.Tags = new HashSet<Tag>();
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PreparationTime { get; set; }

        public int CookTime { get; set; }

        public int NumberOfServings { get; set; }

        public string Directions { get; set; }

        public string PictureUrl { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public decimal Rating { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public  ICollection<Comment> Comment { get; set; }

        public  ICollection<Tag> Tags { get; set; }
    }
}
