using Foodnetic.Models;

namespace Foodnetic.Services.Contracts
{
    public interface IMenuService
    {
        bool CheckIfMenuExist(string username);

        Menu Create(string username);

        Menu GetMenu(string currentUser);
    }
}
