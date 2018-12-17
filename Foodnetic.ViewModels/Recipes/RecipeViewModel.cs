using System.Collections.Generic;
using Foodnetic.ViewModels.Comments;
using Foodnetic.ViewModels.Ingredients;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.ViewModels.Recipes
{
    public class RecipeViewModel
    {
        public RecipeViewModel()
        {
            this.IngredientsViewModel = new List<IngredientsViewModel>();
            this.CommentViewModels = new List<CommentViewModel>();
        }

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

        public ICollection<CommentViewModel> CommentViewModels { get; set; }

        public ICollection<IngredientsViewModel> IngredientsViewModel { get; set; }
    }
}
