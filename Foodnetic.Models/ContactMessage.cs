using System;

namespace Foodnetic.Models
{
    public class ContactMessage
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string Message { get; set; }

        public DateTime SentOn { get; set; }
    }
}
