namespace Foodnetic.ViewModels.Recipes
{
    public class RecipeViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Rating { get; set; }

        public string Author { get; set; }

        public string PictureUrl { get; set; }

        public int  PreparationTime { get; set; }

        public int CookTime { get; set; }

        public int NumberOfServings { get; set; }

        public string Directions { get; set; }
    }
}
