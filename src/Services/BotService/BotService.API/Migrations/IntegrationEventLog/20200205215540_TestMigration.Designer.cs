﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;

namespace BotService.API.Migrations.IntegrationEventLog
{
    [DbContext(typeof(IntegrationEventLogContext))]
    [Migration("20200205215540_TestMigration")]
    partial class TestMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.IntegrationEventLogEntry", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("EventId");

                    b.ToTable("IntegrationEventLog");
                });
#pragma warning restore 612, 618
        }
    }
}
