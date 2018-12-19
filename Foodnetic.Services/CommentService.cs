using System;
using System.Linq;
using Foodnetic.Contants;
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
            var currentUser = (User)this.dbContext.Users.FirstOrDefault(u => u.UserName == username);

            if (string.IsNullOrWhiteSpace(bindingModel.Content))
            {
                bindingModel.Content = string.Format(Constants.Messages.NoContentCommentMsg, bindingModel.Rate);
            }

            var comment = new Comment
            {
                Author = currentUser,
                Content = bindingModel.Content,
                RecipeId = bindingModel.RecipeId,
                PostedOn = DateTime.Now
            };

            this.AddRatingToRecipe(bindingModel);

            this.dbContext.Comments.Add(comment);
            this.dbContext.SaveChanges();
        }

        private void AddRatingToRecipe(CreateCommentViewModel bindingModel)
        {
            var recipe = this.dbContext.Recipes.FirstOrDefault(r => r.Id == bindingModel.RecipeId);

            recipe?.Stars.Add(new Rate
            {
                RateNumber = bindingModel.Rate,
                Recipe = recipe
            });

            this.dbContext.SaveChanges();
        }

        public void DeleteCommentContent(string id)
        {
            var comment = this.dbContext.Comments.FirstOrDefault(x => x.Id == id);

            if (comment == null) return;

            comment.Content = Constants.Messages.ModeratorDeleteCommentContentMsg;
            this.dbContext.SaveChanges();

        }
    }
}
