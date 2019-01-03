using System.ComponentModel.DataAnnotations;

namespace Foodnetic.ViewModels.Comments
{
    public class CreateCommentViewModel
    {
       public string Content { get; set; }

        public string RecipeId { get; set; }

        [Required]
        public int Rate { get; set; }
    }
}
