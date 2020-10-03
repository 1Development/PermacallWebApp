using Microsoft.EntityFrameworkCore.Migrations;

namespace Tools.Migrations
{
    public partial class CharacterRemovedAndItemProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "ItemCaches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Quality",
                table: "ItemCaches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slot",
                table: "ItemCaches",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "Characters",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "ItemCaches");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "ItemCaches");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "ItemCaches");

            migrationBuilder.DropColumn(
                name: "Removed",
                table: "Characters");
        }
    }
}
