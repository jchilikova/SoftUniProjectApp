using Microsoft.EntityFrameworkCore.Migrations;

namespace Foodnetic.App.Data.Migrations
{
    public partial class Dbsets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Products_IngredientId",
                table: "RecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Recipes_RecipeId",
                table: "RecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeMenu_Menus_MenuId",
                table: "RecipeMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeMenu_Recipes_RecipeId",
                table: "RecipeMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTag_Recipes_RecipeId",
                table: "RecipeTag");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTag_Tags_TagId",
                table: "RecipeTag");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGrocery_Products_GroceryId",
                table: "UserGrocery");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGrocery_AspNetUsers_UserId",
                table: "UserGrocery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGrocery",
                table: "UserGrocery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeTag",
                table: "RecipeTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeMenu",
                table: "RecipeMenu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredient",
                table: "RecipeIngredient");

            migrationBuilder.RenameTable(
                name: "UserGrocery",
                newName: "UserGroceries");

            migrationBuilder.RenameTable(
                name: "RecipeTag",
                newName: "RecipeTags");

            migrationBuilder.RenameTable(
                name: "RecipeMenu",
                newName: "RecipeMenus");

            migrationBuilder.RenameTable(
                name: "RecipeIngredient",
                newName: "RecipeIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_UserGrocery_GroceryId",
                table: "UserGroceries",
                newName: "IX_UserGroceries_GroceryId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTag_TagId",
                table: "RecipeTags",
                newName: "IX_RecipeTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeMenu_MenuId",
                table: "RecipeMenus",
                newName: "IX_RecipeMenus_MenuId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroceries",
                table: "UserGroceries",
                columns: new[] { "UserId", "GroceryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags",
                columns: new[] { "RecipeId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeMenus",
                table: "RecipeMenus",
                columns: new[] { "RecipeId", "MenuId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                columns: new[] { "RecipeId", "IngredientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Products_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeMenus_Menus_MenuId",
                table: "RecipeMenus",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeMenus_Recipes_RecipeId",
                table: "RecipeMenus",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Recipes_RecipeId",
                table: "RecipeTags",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroceries_Products_GroceryId",
                table: "UserGroceries",
                column: "GroceryId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroceries_AspNetUsers_UserId",
                table: "UserGroceries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Products_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeMenus_Menus_MenuId",
                table: "RecipeMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeMenus_Recipes_RecipeId",
                table: "RecipeMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Recipes_RecipeId",
                table: "RecipeTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroceries_Products_GroceryId",
                table: "UserGroceries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroceries_AspNetUsers_UserId",
                table: "UserGroceries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroceries",
                table: "UserGroceries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeMenus",
                table: "RecipeMenus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.RenameTable(
                name: "UserGroceries",
                newName: "UserGrocery");

            migrationBuilder.RenameTable(
                name: "RecipeTags",
                newName: "RecipeTag");

            migrationBuilder.RenameTable(
                name: "RecipeMenus",
                newName: "RecipeMenu");

            migrationBuilder.RenameTable(
                name: "RecipeIngredients",
                newName: "RecipeIngredient");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroceries_GroceryId",
                table: "UserGrocery",
                newName: "IX_UserGrocery_GroceryId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTags_TagId",
                table: "RecipeTag",
                newName: "IX_RecipeTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeMenus_MenuId",
                table: "RecipeMenu",
                newName: "IX_RecipeMenu_MenuId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredient",
                newName: "IX_RecipeIngredient_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGrocery",
                table: "UserGrocery",
                columns: new[] { "UserId", "GroceryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeTag",
                table: "RecipeTag",
                columns: new[] { "RecipeId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeMenu",
                table: "RecipeMenu",
                columns: new[] { "RecipeId", "MenuId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredient",
                table: "RecipeIngredient",
                columns: new[] { "RecipeId", "IngredientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Products_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Recipes_RecipeId",
                table: "RecipeIngredient",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeMenu_Menus_MenuId",
                table: "RecipeMenu",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeMenu_Recipes_RecipeId",
                table: "RecipeMenu",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTag_Recipes_RecipeId",
                table: "RecipeTag",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTag_Tags_TagId",
                table: "RecipeTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGrocery_Products_GroceryId",
                table: "UserGrocery",
                column: "GroceryId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGrocery_AspNetUsers_UserId",
                table: "UserGrocery",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
