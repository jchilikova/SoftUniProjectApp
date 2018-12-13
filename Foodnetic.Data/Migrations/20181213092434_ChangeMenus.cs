using Microsoft.EntityFrameworkCore.Migrations;

namespace Foodnetic.Data.Migrations
{
    public partial class ChangeMenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "DailyMenuId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UserId",
                table: "Menus",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId",
                table: "Menus");

            migrationBuilder.AddColumn<string>(
                name: "DailyMenuId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UserId",
                table: "Menus",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
