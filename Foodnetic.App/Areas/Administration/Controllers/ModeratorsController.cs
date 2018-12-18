using Foodnetic.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Areas.Administration.Controllers
{
    public class ModeratorsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IRecipeService recipeService;

        public ModeratorsController(ICommentService commentService, IRecipeService recipeService)
        {
            this.commentService = commentService;
            this.recipeService = recipeService;
        }

        public IActionResult DeleteComment(string id, string recipeId)
        {
            this.commentService.DeleteCommentContent(id);

            return RedirectToAction("Recipe", "Recipes", new{ id = recipeId} );
        }

        public IActionResult DeleteRecipe(string id)
        {
            this.recipeService.DeleteRecipe(id);
            return RedirectToAction("All", "Recipes");
        }
    }
}