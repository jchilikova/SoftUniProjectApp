using Microsoft.EntityFrameworkCore.Migrations;

namespace Foodnetic.Data.Migrations
{
    public partial class AddIsInCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInCreate",
                table: "Recipes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInCreate",
                table: "Recipes");
        }
    }
}
