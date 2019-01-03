using System.ComponentModel.DataAnnotations;

namespace Foodnetic.ViewModels.Account
{
    public class RegisterViewModel
    {
        private const string RequiredError = " is required";
        private const int PasswordMinimumCharacters = 5;
        private const int PasswordMaximumCharacters = 50;
        private const int FirstAndLastNameMinimumCharacters = 2;
        private const int FirstAndLastNameMaximumCharacters = 30;
        private const int UsernameMinimumCharacters = 3;
        private const int UsernameMaximumCharacters = 50;

        private const string UsernameLengthError = "Username should contain at least 3 characters and maximum 50!";
        private const string UsernameCharactersContainsError = "Username may only contain numbers, letters, dashes, underscores, dots, asterisks and tildes!";

        private const string ValidEmailError = "Must be a valid Email!";

        private const string PasswordLengthError = "Must contain at least 5 characters!";
        private const string PasswordsNotMatch = "Passwords does not match!";

        private const string FirstAndLastNameLengthError = " must be at least 2 characters and maximum 30!";

        [Required(ErrorMessage = nameof(Username) + RequiredError)]
        [StringLength(UsernameMaximumCharacters, MinimumLength = UsernameMinimumCharacters, ErrorMessage = UsernameLengthError)]
        [RegularExpression("^[A-Za-z0-9_-~*.]+$", ErrorMessage = UsernameCharactersContainsError)]
        public string Username { get; set; }

        [Required(ErrorMessage = nameof(Email) + RequiredError)]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage =ValidEmailError)]
        public string Email { get; set; }

        [Required(ErrorMessage = nameof(Password) + RequiredError)]
        [StringLength(PasswordMaximumCharacters, MinimumLength = PasswordMinimumCharacters, ErrorMessage = PasswordLengthError)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = nameof(ConfirmPassword) + RequiredError)]
        [Compare(nameof(Password), ErrorMessage = PasswordsNotMatch)]
        [Display(Name="Confirm Password")]
        [StringLength(PasswordMaximumCharacters, MinimumLength = PasswordMinimumCharacters, ErrorMessage = PasswordLengthError)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = nameof(FirstName) + RequiredError)]
        [StringLength(FirstAndLastNameMaximumCharacters, MinimumLength = FirstAndLastNameMinimumCharacters, ErrorMessage = nameof(FirstName) + FirstAndLastNameLengthError)]
        [Display(Name="First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = nameof(LastName) + RequiredError)]
        [StringLength(FirstAndLastNameMaximumCharacters, MinimumLength = FirstAndLastNameMinimumCharacters, ErrorMessage = nameof(LastName) + FirstAndLastNameLengthError)]
        [Display(Name="Last name")]
        public string LastName { get; set; }
    }
}
