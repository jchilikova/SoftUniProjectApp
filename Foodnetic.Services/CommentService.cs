using System;
using System.Collections.Generic;
using System.Linq;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Comments;

namespace Foodnetic.Services
{
    public class CommentService : ICommentService
    {
        private readonly FoodneticDbContext dbContext;

        public CommentService(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(CreateCommentViewModel bindingModel, string username)
        {
            User currentUser = (User)this.dbContext.Users.FirstOrDefault(u => u.UserName == username);

            if (string.IsNullOrWhiteSpace(bindingModel.Content))
            {
                bindingModel.Content = $"Rated {bindingModel.Rate}/5";
            }

            var comment = new Comment
            {
                Author = currentUser,
                Content = bindingModel.Content,
                RecipeId = bindingModel.RecipeId,
                PostedOn = DateTime.Now
            };

            var recipe = this.dbContext.Recipes.FirstOrDefault(r => r.Id == bindingModel.RecipeId);

            recipe?.Stars.Add(new Rate
            {
                RateNumber = bindingModel.Rate,
                Recipe = recipe
            });

            this.dbContext.Comments.Add(comment);
            this.dbContext.SaveChanges();
        }
    }
}
