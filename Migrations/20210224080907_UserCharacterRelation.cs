using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_RPG.Migrations
{
    public partial class UserCharacterRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "usersId",
                table: "characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_characters_usersId",
                table: "characters",
                column: "usersId");

            migrationBuilder.AddForeignKey(
                name: "FK_characters_users_usersId",
                table: "characters",
                column: "usersId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characters_users_usersId",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "IX_characters_usersId",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "usersId",
                table: "characters");
        }
    }
}
