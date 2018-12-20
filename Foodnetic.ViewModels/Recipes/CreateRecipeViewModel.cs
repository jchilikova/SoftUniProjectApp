using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foodnetic.ViewModels.Ingredients;
using Microsoft.AspNetCore.Http;

namespace Foodnetic.ViewModels.Recipes
{
    public class CreateRecipeViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int PreparationTime { get; set; }

        public int CookTime { get; set; }

        public int NumberOfServings { get; set; }

        public string Directions { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        public ICollection<IngredientsViewModel> IngredientsViewModels { get; set; }
    }
}
