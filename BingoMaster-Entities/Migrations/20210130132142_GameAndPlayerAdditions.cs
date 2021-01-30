using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BingoMaster_Entities.Migrations
{
    public partial class GameAndPlayerAdditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumAmountOfPlayers",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WinnerId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_CreatorId",
                table: "Games",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinnerId",
                table: "Games",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_CreatorId",
                table: "Games",
                column: "CreatorId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_WinnerId",
                table: "Games",
                column: "WinnerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_CreatorId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_WinnerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_CreatorId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_WinnerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MaximumAmountOfPlayers",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Games");
        }
    }
}
