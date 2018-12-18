using Foodnetic.Models;
using Foodnetic.ViewModels.Comments;

namespace Foodnetic.Services.Contracts
{
    public interface ICommentService
    {
        void Create(CreateCommentViewModel bindingModel, string username);
        void DeleteCommentContent(string id);
    }
}
