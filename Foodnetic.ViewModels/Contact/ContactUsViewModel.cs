using System.ComponentModel.DataAnnotations;

namespace Foodnetic.ViewModels.Contact
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Must be a valid Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required!")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Message must contain at least 2 characters and maximum 200!")]
        public string Message { get; set; }
    }
}
