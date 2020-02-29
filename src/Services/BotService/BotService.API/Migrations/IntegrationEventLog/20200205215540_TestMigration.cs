using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BotService.API.Migrations.IntegrationEventLog
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "IntegrationEventLog");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "IntegrationEventLog");

            migrationBuilder.DropColumn(
                name: "EventTypeName",
                table: "IntegrationEventLog");

            migrationBuilder.DropColumn(
                name: "State",
                table: "IntegrationEventLog");

            migrationBuilder.DropColumn(
                name: "TimesSent",
                table: "IntegrationEventLog");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "IntegrationEventLog");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "IntegrationEventLog",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "IntegrationEventLog",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "IntegrationEventLog",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "IntegrationEventLog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EventTypeName",
                table: "IntegrationEventLog",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "IntegrationEventLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesSent",
                table: "IntegrationEventLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "IntegrationEventLog",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
