using Foodnetic.Constants;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Areas.Recipes.Controllers
{
    [Area(GlobalConstants.RecipesAreaString)]
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateCommentViewModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                var currentUsername = this.User.Identity.Name;

                this.commentService.Create(bindingModel, currentUsername);

                return RedirectToAction("Recipe", "Recipes", new {Id = bindingModel.RecipeId});
            }

            return RedirectToAction("Recipe", "Recipes", new {Id = bindingModel.RecipeId});
        }
    }
}