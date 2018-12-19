using System;

namespace Foodnetic.ViewModels.Contact
{
    public class AllContactUsMessagesViewModel
    {
        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Message { get; set; }

        public DateTime SentOn { get; set; }
    }
}
