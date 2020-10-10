using Microsoft.EntityFrameworkCore.Migrations;

namespace Tools.Migrations
{
    public partial class ClassRacePlayerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "CharacterEquipmentCaches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Race",
                table: "CharacterEquipmentCaches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "CharacterEquipmentCaches");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "CharacterEquipmentCaches");
        }
    }
}
