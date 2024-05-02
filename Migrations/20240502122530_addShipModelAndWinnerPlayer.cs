using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class addShipModelAndWinnerPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameInstancePlayer");

            migrationBuilder.RenameColumn(
                name: "victor",
                table: "GameInstances",
                newName: "WinnerId");

            migrationBuilder.AddColumn<int>(
                name: "GameInstanceId",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShipType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipTypeId = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Vertical = table.Column<bool>(type: "bit", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ship_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ship_ShipType_ShipTypeId",
                        column: x => x.ShipTypeId,
                        principalTable: "ShipType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameInstanceId",
                table: "Player",
                column: "GameInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GameInstances_WinnerId",
                table: "GameInstances",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_BoardId",
                table: "Ship",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_ShipTypeId",
                table: "Ship",
                column: "ShipTypeId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameInstances_Player_WinnerId",
                table: "GameInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_GameInstances_GameInstanceId",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Ship");

            migrationBuilder.DropTable(
                name: "ShipType");

            migrationBuilder.DropIndex(
                name: "IX_Player_GameInstanceId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_GameInstances_WinnerId",
                table: "GameInstances");

            migrationBuilder.DropColumn(
                name: "GameInstanceId",
                table: "Player");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "GameInstances",
                newName: "victor");

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
    }
}
