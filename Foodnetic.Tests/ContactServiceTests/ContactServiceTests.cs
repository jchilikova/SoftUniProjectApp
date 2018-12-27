using System;
using System.Linq;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Tests.ContactServiceTests
{
    public class ContactServiceTests : BaseService
    {
        private IContactService ContactService => this.ServiceProvider.GetRequiredService<IContactService>();

        [Test]
        public void CreateContactMessageShouldAddContactMessageToDatabase()
        {
            var contactMessage = new ContactMessage
            {
                Message = "Test",
                UserEmail = "test@test.test",
                UserName = "Test"
            };

            this.ContactService.CreateContactMessage(contactMessage);

            var count = this.DbContext.ContactMessages.Count();

            Assert.AreEqual(count, 1);
        }

        [Test]
        public void GetAllShouldReturnAllContactMessagesFromDatabase()
        {
            var contactMessage = new ContactMessage
            {
                Message = "Test",
                UserEmail = "test@test.test",
                UserName = "Test"
            };

            var contactMessage2 = new ContactMessage
            {
                Message = "Test2",
                UserEmail = "test2@test.test",
                UserName = "Test2"
            };

            this.DbContext.ContactMessages.Add(contactMessage);
            this.DbContext.ContactMessages.Add(contactMessage2);
            this.DbContext.SaveChanges();

            var count = this.ContactService.GetAll().Count();

            Assert.AreEqual(count, 2);
        }

        [Test]
        public void GetAllShouldReturnNullIfNoMessagesExist()
        {
            var result = this.ContactService.GetAll();

            Assert.AreEqual(result, null);
        }
    }
}
