using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Foodnetic.Models.Enums;

namespace Foodnetic.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Ingredients = new HashSet<Ingredient>();
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<RecipeTag>();
            this.Id = Guid.NewGuid().ToString();
            this.Stars = new HashSet<Rate>();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int PreparationTime { get; set; }

        [Required]
        public int CookTime { get; set; }

        [Required]
        public int NumberOfServings { get; set; }

        [Required]
        public string Directions { get; set; }

        [Required]
        public DishType DishType { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public bool IsInCreate { get; set; } = false;

        public string AuthorId { get; set; }
        public FoodneticUser Author { get; set; }

        public decimal Rating => (decimal) (this.Stars.Count == 0 ? 0 : this.Stars.Select(s => s.RateNumber).Average());

        public bool IsDeleted { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public ICollection<Rate> Stars { get; set; }

        public ICollection<RecipeMenu> Menus { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<RecipeTag> Tags { get; set; }
    }
}
