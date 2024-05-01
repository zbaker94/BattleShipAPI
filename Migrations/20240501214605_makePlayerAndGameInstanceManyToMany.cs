using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class makePlayerAndGameInstanceManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
    }
}
