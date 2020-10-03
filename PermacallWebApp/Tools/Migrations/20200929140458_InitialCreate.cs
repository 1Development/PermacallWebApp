using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tools.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Realm = table.Column<string>(nullable: true),
                    AddedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterEquipmentCaches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: true),
                    CacheTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterEquipmentCaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterEquipmentCaches_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemCaches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterEquipmentCacheId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCaches_CharacterEquipmentCaches_CharacterEquipmentCacheId",
                        column: x => x.CharacterEquipmentCacheId,
                        principalTable: "CharacterEquipmentCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterEquipmentCaches_CharacterId",
                table: "CharacterEquipmentCaches",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCaches_CharacterEquipmentCacheId",
                table: "ItemCaches",
                column: "CharacterEquipmentCacheId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCaches");

            migrationBuilder.DropTable(
                name: "CharacterEquipmentCaches");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
