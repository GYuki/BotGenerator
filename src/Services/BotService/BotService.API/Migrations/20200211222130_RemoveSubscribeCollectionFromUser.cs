using Microsoft.EntityFrameworkCore.Migrations;

namespace BotService.API.Migrations
{
    public partial class RemoveSubscribeCollectionFromUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribe_User_UserId",
                table: "Subscribe");

            migrationBuilder.DropIndex(
                name: "IX_Subscribe_UserId",
                table: "Subscribe");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Subscribe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Subscribe",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribe_UserId",
                table: "Subscribe",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribe_User_UserId",
                table: "Subscribe",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
