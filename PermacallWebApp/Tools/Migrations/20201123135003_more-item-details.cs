using Microsoft.EntityFrameworkCore.Migrations;

namespace Tools.Migrations
{
    public partial class moreitemdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bonus",
                table: "ItemCaches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Enchants",
                table: "ItemCaches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gems",
                table: "ItemCaches",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "ItemCaches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "ItemCaches");

            migrationBuilder.DropColumn(
                name: "Enchants",
                table: "ItemCaches");

            migrationBuilder.DropColumn(
                name: "Gems",
                table: "ItemCaches");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemCaches");
        }
    }
}
