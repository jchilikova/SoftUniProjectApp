using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Foodnetic.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> ExternalLoginUser(ExternalLoginInfo info);
    }
}
