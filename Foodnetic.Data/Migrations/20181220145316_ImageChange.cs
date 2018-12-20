using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Foodnetic.Data.Migrations
{
    public partial class ImageChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Recipes");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Recipes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Recipes",
                nullable: true);
        }
    }
}
