using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyGameInstanceToHavePlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_GameInstances_GameInstanceId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_GameInstanceId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "GameInstanceId",
                table: "Player");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "GameInstances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameInstances_PlayerId",
                table: "GameInstances",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameInstances_Player_PlayerId",
                table: "GameInstances",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameInstances_Player_PlayerId",
                table: "GameInstances");

            migrationBuilder.DropIndex(
                name: "IX_GameInstances_PlayerId",
                table: "GameInstances");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "GameInstances");

            migrationBuilder.AddColumn<int>(
                name: "GameInstanceId",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameInstanceId",
                table: "Player",
                column: "GameInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_GameInstances_GameInstanceId",
                table: "Player",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id");
        }
    }
}
