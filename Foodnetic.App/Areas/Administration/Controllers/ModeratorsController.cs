using Foodnetic.Constants;
using Foodnetic.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Areas.Administration.Controllers
{
    [Area(GlobalConstants.AdministrationAreaString)]
    public class ModeratorsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IRecipeService recipeService;

        public ModeratorsController(ICommentService commentService, IRecipeService recipeService)
        {
            this.commentService = commentService;
            this.recipeService = recipeService;
        }

        [Authorize(Roles = GlobalConstants.ModeratorRole)]
        public IActionResult DeleteComment(string id, string recipeId)
        {
            this.commentService.AlterCommentContent(id);

            return RedirectToAction("Recipe", "Recipes", new{ id = recipeId, area = "Recipes" } );
        }

        [Authorize(Roles = GlobalConstants.ModeratorRole)]
        public IActionResult DeleteRecipe(string id)
        {
            this.recipeService.DeleteRecipe(id);
            return RedirectToAction("All", "Recipes",  new{ area = "Recipes" });
        }
    }
}