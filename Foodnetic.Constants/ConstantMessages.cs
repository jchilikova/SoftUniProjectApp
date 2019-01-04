namespace Foodnetic.Infrastructure
{
    public static class ConstantMessages
    {
        public const string NotEnoughGroceriesErrorMsg = "You don't have enough groceries for creating a menu. Try add some groceries and create menu again!";
        public const string AddIngredientFirstMsg = "You need to add ingredients first!";
        public const string ProductAlreadyExistsErrorMsg = "Product with that name already exists!";

        public const string UserAlreadyExistsErrorMsg = "User with that username and password does not exists!";
        public const string UsernameAlreadyExistsErrorMsg = "User with that username already exists!";
        public const string EmailAlreadyExistsErrorMsg = "User with that email already exists!";

        public const string PromotedUserMsg = "promoted to moderator successfully!";
        public const string DemotedUserMsg = "demoted from moderator successfully!";
        public const string ModeratorDeleteCommentContentMsg = "Comment is deleted by Moderator";

        public const string InvalidRecipeMsgError = "Invalid recipe!";

        public const string NoContentCommentMsg = "Rated {0}/5";

        public const string InvalidDataMsg = "Invalid data";
        public const string MenuForTodayAlreadyExists = "You already have menu for today. Come back tomorrow!";
    }
}

