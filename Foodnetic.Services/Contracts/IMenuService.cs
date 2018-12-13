using System.Collections.Generic;
using Foodnetic.Models;

namespace Foodnetic.Services.Contracts
{
    public interface IMenuService
    {
        bool CheckIfMenuExist(string username);

        Menu Create(string username);

        Menu GetDailyMenuForUser(string currentUser);

        ICollection<Menu> GetAllMenusForUser(string currentUser);
    }
}
