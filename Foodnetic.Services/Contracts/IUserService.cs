using System.Collections.Generic;
using System.Threading.Tasks;
using Foodnetic.Models;
using Foodnetic.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> ExternalLoginUser(ExternalLoginInfo info);

        Task<bool> SignInUser(LoginViewModel bindingModel);

        bool CheckIfUserExists(string username);

        bool CheckIfEmailExists(string email);

        FoodneticUser CreateUser(RegisterViewModel bindingModel);

        Task AddToRole(FoodneticUser user);

        IEnumerable<FoodneticUser> GetAll();

        Task<string> DemoteFromModerator(string id);

        Task<string> MakeModerator(string id);
    }
}
