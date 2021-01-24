using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BingoMaster_Entities.Migrations
{
    public partial class GamePlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayer_Games_GameId",
                table: "GamePlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayer_Players_PlayerId",
                table: "GamePlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayer",
                table: "GamePlayer");

            migrationBuilder.RenameTable(
                name: "GamePlayer",
                newName: "GamePlayers");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayer_GameId_PlayerId",
                table: "GamePlayers",
                newName: "IX_GamePlayers_GameId_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayer_PlayerId",
                table: "GamePlayers",
                newName: "IX_GamePlayers_PlayerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Games",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Games_GameId",
                table: "GamePlayers",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Players_PlayerId",
                table: "GamePlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Games_GameId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Players_PlayerId",
                table: "GamePlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "GamePlayers",
                newName: "GamePlayer");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_GameId_PlayerId",
                table: "GamePlayer",
                newName: "IX_GamePlayer_GameId_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_PlayerId",
                table: "GamePlayer",
                newName: "IX_GamePlayer_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayer",
                table: "GamePlayer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayer_Games_GameId",
                table: "GamePlayer",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayer_Players_PlayerId",
                table: "GamePlayer",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
