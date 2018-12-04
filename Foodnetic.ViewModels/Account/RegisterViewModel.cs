﻿using System.ComponentModel.DataAnnotations;

namespace Foodnetic.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required!")]
        [Display(Name="Username")]
        [MinLength(3, ErrorMessage = "Username should contain at least 3 characters!")]
        [RegularExpression("^[A-Za-z0-9_-~*.]+$", ErrorMessage = "Username may only contain numbers, letters, dashes, underscores, dots, asterisks and tildes!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [Display(Name="Email")]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Must be a valid Email!")]
        public string Email { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Must contain at least 5 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords does not match!")]
        [Display(Name="Confirm Password")]
        [MinLength(5, ErrorMessage = "Must contain at least 5 characters!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required!")]
        [Display(Name="First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [Display(Name="Last name")]
        public string LastName { get; set; }
    }
}