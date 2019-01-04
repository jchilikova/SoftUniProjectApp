using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.Models.Enums;
using Foodnetic.ViewModels.Ingredients;
using Microsoft.AspNetCore.Http;

namespace Foodnetic.ViewModels.Recipes
{
    public class CreateRecipeViewModel
    {
        private const string RequiredError = " is required";
        private const int NameMinimumLength = 3;
        private const int NameMaximumLength = 50;

        private const int DescriptionMinimumLength = 5;
        private const int DescriptionMaximumLength = 250;

        private const int DirectionsMinimumLength = 10;
        private const int DirectionsMaximumLength = 2000;

        private const int TagsMinimumLength = 3;
        private const int TagsMaximumLength = 250;

        private const int PrepAndCookTimeMinimumMinutes = 1;
        private const int PrepAndCookTimeMaxMinutes = 500;

        private const int NumberOfServingsMaximum = 15;

        private const string NumberOfServingsString = "Number of servings";


        [Required(ErrorMessage = nameof(Name) + RequiredError)]
        [StringLength(NameMaximumLength, MinimumLength = NameMinimumLength, ErrorMessage = "Recipe name must contain maximum 50 characters and minimum 3!")]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(Description) + RequiredError)]
        [StringLength(DescriptionMaximumLength, MinimumLength = DescriptionMinimumLength, ErrorMessage = "Description must contain maximum 250 characters and minimum 5!")]
        public string Description { get; set; }

        [Required(ErrorMessage = nameof(PreparationTime) + RequiredError)]
        [Range(PrepAndCookTimeMinimumMinutes, PrepAndCookTimeMaxMinutes, ErrorMessage = "Prep time must be in range 1 to 300 minutes.")]
        public int PreparationTime { get; set; }

        [Required(ErrorMessage = nameof(CookTime) + RequiredError)]
        [Range(PrepAndCookTimeMinimumMinutes, PrepAndCookTimeMaxMinutes, ErrorMessage = "Cook time must be in range 1 to 500 minutes.")]
        public int CookTime { get; set; }

        [Required(ErrorMessage = NumberOfServingsString + RequiredError)]
        [Display(Name = NumberOfServingsString)]
        [Range(PrepAndCookTimeMinimumMinutes, NumberOfServingsMaximum, ErrorMessage = "Number of servings must be in range 1 to 10.")]
        public int NumberOfServings { get; set; }

        [Required(ErrorMessage = nameof(Directions) + RequiredError)]
        [StringLength(DirectionsMaximumLength, ErrorMessage = "Your directions must contain maximum 2000 characters and at least 10 characters!!",
            MinimumLength = DirectionsMinimumLength)]
        public string Directions { get; set; }

        [Required(ErrorMessage = nameof(Tags) + RequiredError)]
        [StringLength(TagsMaximumLength, MinimumLength = TagsMinimumLength, ErrorMessage = "Tags must contain maximum 1000 characters and minimum 3!")]
        public string Tags { get; set; }

        [Required(ErrorMessage = nameof(DishType) + RequiredError)]
        public DishType DishType { get; set; }

        [Required(ErrorMessage = nameof(Image) + RequiredError)]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        public ICollection<IngredientsViewModel> IngredientsViewModels { get; set; }
    }
}
