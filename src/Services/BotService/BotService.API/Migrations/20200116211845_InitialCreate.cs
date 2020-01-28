using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BotService.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SenderId = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bot_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Command",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Request = table.Column<string>(maxLength: 20, nullable: false),
                    Response = table.Column<string>(maxLength: 200, nullable: false),
                    BotId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Command_Bot_BotId",
                        column: x => x.BotId,
                        principalTable: "Bot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscribe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BotId = table.Column<int>(nullable: false),
                    ChatId = table.Column<string>(maxLength: 100, nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribe_Bot_BotId",
                        column: x => x.BotId,
                        principalTable: "Bot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscribe_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bot_Name",
                table: "Bot",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bot_OwnerId",
                table: "Bot",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bot_Token",
                table: "Bot",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Command_BotId_Request",
                table: "Command",
                columns: new[] { "BotId", "Request" });

            migrationBuilder.CreateIndex(
                name: "IX_Subscribe_UserId",
                table: "Subscribe",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribe_BotId_ChatId",
                table: "Subscribe",
                columns: new[] { "BotId", "ChatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_SenderId",
                table: "User",
                column: "SenderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Command");

            migrationBuilder.DropTable(
                name: "Subscribe");

            migrationBuilder.DropTable(
                name: "Bot");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
