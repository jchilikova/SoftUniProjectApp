using System;
using System.Collections.Generic;
using System.Linq;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;

namespace Foodnetic.Services
{
    public class ContactService : IContactService
    {
        private readonly FoodneticDbContext dbContext;

        public ContactService(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ContactMessage> GetAll()
        {
            if (this.dbContext.ContactMessages.Any())
            {
                return this.dbContext.ContactMessages.OrderBy(x => x.SentOn);
            }

            return null;

        }

        public void CreateContactMessage(ContactMessage contactMessage)
        {
            contactMessage.SentOn = DateTime.UtcNow;
            this.dbContext.ContactMessages.Add(contactMessage);
            this.dbContext.SaveChanges();
        }
    }
}
