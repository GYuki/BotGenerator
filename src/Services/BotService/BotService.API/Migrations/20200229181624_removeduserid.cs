using Microsoft.EntityFrameworkCore.Migrations;

namespace BotService.API.Migrations
{
    public partial class removeduserid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bot_OwnerId",
                table: "Bot");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Bot");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Bot",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bot_OwnerId",
                table: "Bot",
                column: "OwnerId");
        }
    }
}
