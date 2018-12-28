using Microsoft.EntityFrameworkCore.Migrations;

namespace Foodnetic.Data.Migrations
{
    public partial class ChangeRecipeDishType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuType",
                table: "RecipeMenus");

            migrationBuilder.AddColumn<int>(
                name: "DishType",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DishType",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "MenuType",
                table: "RecipeMenus",
                nullable: false,
                defaultValue: 0);
        }
    }
}
