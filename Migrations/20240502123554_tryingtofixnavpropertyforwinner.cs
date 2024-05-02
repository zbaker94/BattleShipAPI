using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class tryingtofixnavpropertyforwinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameInstances_Player_WinnerId",
                table: "GameInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_GameInstances_GameInstanceId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_GameInstanceId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_GameInstances_WinnerId",
                table: "GameInstances");

            migrationBuilder.DropColumn(
                name: "GameInstanceId",
                table: "Player");

            migrationBuilder.CreateTable(
                name: "GameInstancePlayer",
                columns: table => new
                {
                    GameInstancesId = table.Column<int>(type: "int", nullable: false),
                    PlayersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameInstancePlayer", x => new { x.GameInstancesId, x.PlayersId });
                    table.ForeignKey(
                        name: "FK_GameInstancePlayer_GameInstances_GameInstancesId",
                        column: x => x.GameInstancesId,
                        principalTable: "GameInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameInstancePlayer_Player_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameInstancePlayer_PlayersId",
                table: "GameInstancePlayer",
                column: "PlayersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameInstancePlayer");

            migrationBuilder.AddColumn<int>(
                name: "GameInstanceId",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameInstanceId",
                table: "Player",
                column: "GameInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GameInstances_WinnerId",
                table: "GameInstances",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameInstances_Player_WinnerId",
                table: "GameInstances",
                column: "WinnerId",
                principalTable: "Player",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_GameInstances_GameInstanceId",
                table: "Player",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id");
        }
    }
}
