﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tools.Database;

namespace Tools.Migrations
{
    [DbContext(typeof(ToolContext))]
    [Migration("20200929140458_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Tools.Database.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Realm")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Tools.Database.Models.CharacterEquipmentCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CacheTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CharacterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("CharacterEquipmentCaches");
                });

            modelBuilder.Entity("Tools.Database.Models.ItemCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CharacterEquipmentCacheId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterEquipmentCacheId");

                    b.ToTable("ItemCaches");
                });

            modelBuilder.Entity("Tools.Database.Models.CharacterEquipmentCache", b =>
                {
                    b.HasOne("Tools.Database.Models.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId");
                });

            modelBuilder.Entity("Tools.Database.Models.ItemCache", b =>
                {
                    b.HasOne("Tools.Database.Models.CharacterEquipmentCache", null)
                        .WithMany("Items")
                        .HasForeignKey("CharacterEquipmentCacheId");
                });
#pragma warning restore 612, 618
        }
    }
}
