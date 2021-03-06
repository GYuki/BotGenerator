﻿// <auto-generated />
using System;
using BotService.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BotService.API.Migrations
{
    [DbContext(typeof(BotContext))]
    [Migration("20200211220039_ChatIdTypeAndRemoveUserId")]
    partial class ChatIdTypeAndRemoveUserId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BotService.API.Model.Bot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("Bot");
                });

            modelBuilder.Entity("BotService.API.Model.Subscribe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BotId")
                        .HasColumnType("int");

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("BotId", "ChatId")
                        .IsUnique();

                    b.ToTable("Subscribe");
                });

            modelBuilder.Entity("BotService.API.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("SenderId")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("BotService.API.Model.Bot", b =>
                {
                    b.HasOne("BotService.API.Model.User", "Owner")
                        .WithMany("Bots")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BotService.API.Model.Subscribe", b =>
                {
                    b.HasOne("BotService.API.Model.Bot", "Bot")
                        .WithMany("Subscribes")
                        .HasForeignKey("BotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BotService.API.Model.User", null)
                        .WithMany("Subscribes")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
