using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Foodnetic.Data.Migrations
{
    public partial class ChangeComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Comments");

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedOn",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedOn",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Comments",
                nullable: true);
        }
    }
}
