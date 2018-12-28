using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models.Enums;
using Foodnetic.ViewModels.Ingredients;
using Microsoft.AspNetCore.Http;

namespace Foodnetic.ViewModels.Recipes
{
    public class CreateRecipeViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Recipe name must contain maximum 50 characters and minimum 3!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(150, ErrorMessage = "Your description must contain maximum 150 characters!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Prep Time is required!")]
        [Range(1, 300, ErrorMessage = "Prep time must be in range 1 to 300 minutes.")]
        public int PreparationTime { get; set; }

        [Required(ErrorMessage = "Cook Time is required!")]
        [Range(1, 500, ErrorMessage = "Cook time must be in range 1 to 500 minutes.")]
        public int CookTime { get; set; }

        [Required(ErrorMessage = "Number of servings is required!")]
        [Display(Name = "Number of servings")]
        [Range(1, 10, ErrorMessage = "Number of servings must be in range 1 to 10.")]
        public int NumberOfServings { get; set; }

        [Required(ErrorMessage = "Directions are required!")]
        [StringLength(2000, ErrorMessage = "Your directions must contain maximum 2000 characters and at least 10 characters!!",
            MinimumLength = 10)]
        public string Directions { get; set; }

        [Required(ErrorMessage = "Tags are required!")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Your description must contain maximum 1000 characters and minimum 3!")]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Dish Type is required!")]
        public DishType DishType { get; set; }

        [Required(ErrorMessage = "Image is required!")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        public ICollection<IngredientsViewModel> IngredientsViewModels { get; set; }
    }
}
