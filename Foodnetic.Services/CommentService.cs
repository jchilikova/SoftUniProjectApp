using System;
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

            var comment = new Comment
            {
                Author = currentUser,
                Content = bindingModel.Content,
                RecipeId = bindingModel.RecipeId,
                PostedOn = DateTime.Now
            };

            this.dbContext.Comments.Add(comment);
            this.dbContext.SaveChanges();
        }
    }
}
