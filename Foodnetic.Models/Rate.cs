namespace Foodnetic.Models
{
    public class Rate
    {
        public string Id { get; set; }

        public int RateNumber { get; set; }

        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }  
    }
}
