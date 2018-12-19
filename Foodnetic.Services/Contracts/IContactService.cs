using System.Collections.Generic;
using Foodnetic.Models;

namespace Foodnetic.Services.Contracts
{
    public interface IContactService
    {
        void SendContactMessage(ContactMessage contactMessage);

        IEnumerable<ContactMessage> GetAll();
    }
}
