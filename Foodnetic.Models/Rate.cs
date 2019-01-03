using System.ComponentModel.DataAnnotations;

namespace Foodnetic.Models
{
    public class Rate
    {
        public string Id { get; set; }

        [Required]
        public int RateNumber { get; set; }

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }  
    }
}
