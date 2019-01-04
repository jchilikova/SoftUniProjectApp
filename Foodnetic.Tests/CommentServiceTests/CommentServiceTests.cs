using System.Linq;
using Foodnetic.Infrastructure;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Comments;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Services.Tests.CommentServiceTests
{
    public class CommentServiceTests : BaseService
    {
        private ICommentService CommentService => this.ServiceProvider.GetRequiredService<ICommentService>();

        [Test]
        public void CreateCommentShouldAddCommentToRecipe()
        {
            var recipe = new Recipe
            {
                Id = "1"
            };

            var user = new FoodneticUser()
            {
                UserName = "Test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var commentBindingModel = new CreateCommentViewModel()
            {
                Content = "Test Text",
                Rate = 4,
                RecipeId = "1"
            };

            this.CommentService.Create(commentBindingModel, "Test");

            var count = this.DbContext.Comments.Count();

            Assert.AreEqual(count, 1);
        }

        [Test]
        public void CreateCommentShouldAddCommentToRecipeEvenIfThereIsNoContent()
        {
            var recipe = new Recipe
            {
                Id = "1"
            };

            var user = new FoodneticUser()
            {
                UserName = "Test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.Recipes.Add(recipe);
            this.DbContext.SaveChanges();

            var commentBindingModel = new CreateCommentViewModel()
            {
                Content = "",
                Rate = 4,
                RecipeId = "1"
            };

            this.CommentService.Create(commentBindingModel, "Test");

            var count = this.DbContext.Comments.Count();

            Assert.AreEqual(count, 1);
        }

        [Test]
        public void AlterCommentContentShouldAlterCommentContent()
        {
            var content = "test";

            var comment = new Comment
            {
                Content = content,
                Id = "1"
            };

            this.DbContext.Comments.Add(comment);
            this.DbContext.SaveChanges();

           this.CommentService.AlterCommentContent("1");
            var result = this.DbContext.Comments.FirstOrDefault()?.Content;

            Assert.AreEqual(result, ConstantMessages.ModeratorDeleteCommentContentMsg);
        }

        [Test]
        public void AlterCommentContentShouldDoNothingIfNoCommentExists()
        { 
            var content = "test";

            var comment = new Comment
            {
                Content = content,
                Id = "1"
            };

            this.DbContext.Comments.Add(comment);
            this.DbContext.SaveChanges();
            this.CommentService.AlterCommentContent("2");
            var result = this.DbContext.Comments.FirstOrDefault()?.Content;

            Assert.AreEqual(result, content);
        }
    }
}
