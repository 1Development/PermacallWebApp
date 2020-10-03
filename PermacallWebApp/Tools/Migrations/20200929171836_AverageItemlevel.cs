using Microsoft.EntityFrameworkCore.Migrations;

namespace Tools.Migrations
{
    public partial class AverageItemlevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AverageItemLevel",
                table: "CharacterEquipmentCaches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EquippedItemLevel",
                table: "CharacterEquipmentCaches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageItemLevel",
                table: "CharacterEquipmentCaches");

            migrationBuilder.DropColumn(
                name: "EquippedItemLevel",
                table: "CharacterEquipmentCaches");
        }
    }
}
