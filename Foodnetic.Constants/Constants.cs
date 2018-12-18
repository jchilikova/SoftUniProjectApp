namespace Foodnetic.Contants
{
    public static class Constants
    {
        public static class Messages
        {
            public const string NotEnoughGroceriesErrorMsg =  "You don't have enough groceries for creating a menu. Try add some groceries and create menu again!";
            public const string ProductAlreadyExistsErrorMsg = "Product with that name already exists!";
            public const string UserAlreadyExistsErrorMsg = "User with that username and password does not exists!";
            public const string UsernameAlreadyExistsErrorMsg = "User with that username already exists!";
            public const string EmailAlreadyExistsErrorMsg = "User with that email already exists!";
            public const string PromotedUserMsg = "promoted to moderator successfully!";
            public const string DemotedUserMsg = "demoted from moderator successfully!";
            public const string InvalidRecipeMsgError = "Invalid recipe!";
            public const string ModeratorDeleteCommentContentMsg = "Comment is deleted by Moderator";
            public const string NoContentCommentMsg = "Rated {0}/5";
        }

        public static class Strings
        {
            public const string AdministratorRole = "Administrator";

            public const string AdminString = "admin";

            public const string UserRole = "User";

            public const string ModeratorRole = "Moderator";

            public const string BreakfastString = "breakfast";

            public const string LunchString = "lunch";

            public const string DinnerString = "dinner";

            public const string DessertString = "dessert";

            public const string ErrorString = "Error";

            public const string ExternalLoginRedirect = "/Account/ExternalLogin";

            public const string SuccessString = "Success";
        }
    }
}
