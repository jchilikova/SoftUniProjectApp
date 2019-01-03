using System.ComponentModel.DataAnnotations;

namespace Foodnetic.ViewModels.Account
{
    public class LoginViewModel
    {
        private const string RequiredError = " is required";
        private const string RememberMeString = "Remember me?";

        [Required(ErrorMessage = nameof(Username) + RequiredError)]
        public string Username { get; set; }

        [Required(ErrorMessage =  nameof(Password) + RequiredError)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = RememberMeString)]
        public bool RememberMe { get; set; }
    }
}
