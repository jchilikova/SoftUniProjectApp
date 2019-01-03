using System.ComponentModel.DataAnnotations;

namespace Foodnetic.ViewModels.Contact
{
    public class ContactUsViewModel
    {
        private const string RequiredError = " is required";
        private const string ValidEmailError = "Must be a valid Email!";
        private const int MessageMaximumCharacters = 200;
        private const int MessageMinimumCharacters = 2;

        [Required(ErrorMessage = nameof(Name) + RequiredError)]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(Email) + RequiredError)]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = ValidEmailError)]
        public string Email { get; set; }

        [Required(ErrorMessage = nameof(Message) + RequiredError)]
        [StringLength(MessageMaximumCharacters, MinimumLength = MessageMinimumCharacters, ErrorMessage = "Message must contain at least 2 characters and maximum 200!")]
        public string Message { get; set; }
    }
}
