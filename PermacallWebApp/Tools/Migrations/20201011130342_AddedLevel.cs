using Microsoft.EntityFrameworkCore.Migrations;

namespace Tools.Migrations
{
    public partial class AddedLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "CharacterEquipmentCaches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "CharacterEquipmentCaches");
        }
    }
}
