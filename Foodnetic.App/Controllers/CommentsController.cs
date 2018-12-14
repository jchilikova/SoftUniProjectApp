﻿using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Comments;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        public IActionResult Create(CreateCommentViewModel bindingModel)
        {
            var currentUsername = this.User.Identity.Name;

            this.commentService.Create(bindingModel, currentUsername);

            return RedirectToAction("Recipe", "Recipes", new {Id = bindingModel.RecipeId});
        }
    }
}