using Microsoft.EntityFrameworkCore.Migrations;

namespace BingoMaster_Entities.Migrations
{
    public partial class GamePrivate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CenterSquareFree",
                table: "Games",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Private",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenterSquareFree",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Private",
                table: "Games");
        }
    }
}
