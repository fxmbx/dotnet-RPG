using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_RPG.Migrations
{
    public partial class FigthProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Defeats",
                table: "characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Fight",
                table: "characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Victories",
                table: "characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defeats",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "Fight",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "Victories",
                table: "characters");
        }
    }
}
