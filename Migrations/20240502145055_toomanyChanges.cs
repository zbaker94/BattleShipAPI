using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class toomanyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ship_Boards_BoardId",
                table: "Ship");

            migrationBuilder.DropForeignKey(
                name: "FK_Ship_ShipType_ShipTypeId",
                table: "Ship");

            migrationBuilder.DropForeignKey(
                name: "FK_Shot_Boards_BoardId",
                table: "Shot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shot",
                table: "Shot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipType",
                table: "ShipType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ship",
                table: "Ship");

            migrationBuilder.RenameTable(
                name: "Shot",
                newName: "Shots");

            migrationBuilder.RenameTable(
                name: "ShipType",
                newName: "ShipTypes");

            migrationBuilder.RenameTable(
                name: "Ship",
                newName: "Ships");

            migrationBuilder.RenameIndex(
                name: "IX_Shot_BoardId",
                table: "Shots",
                newName: "IX_Shots_BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Ship_ShipTypeId",
                table: "Ships",
                newName: "IX_Ships_ShipTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ship_BoardId",
                table: "Ships",
                newName: "IX_Ships_BoardId");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivePlayerId",
                table: "GameInstances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shots",
                table: "Shots",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ships",
                table: "Ships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_Boards_BoardId",
                table: "Ships",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships",
                column: "ShipTypeId",
                principalTable: "ShipTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shots_Boards_BoardId",
                table: "Shots",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ships_Boards_BoardId",
                table: "Ships");

            migrationBuilder.DropForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships");

            migrationBuilder.DropForeignKey(
                name: "FK_Shots_Boards_BoardId",
                table: "Shots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shots",
                table: "Shots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ships",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "ActivePlayerId",
                table: "GameInstances");

            migrationBuilder.RenameTable(
                name: "Shots",
                newName: "Shot");

            migrationBuilder.RenameTable(
                name: "ShipTypes",
                newName: "ShipType");

            migrationBuilder.RenameTable(
                name: "Ships",
                newName: "Ship");

            migrationBuilder.RenameIndex(
                name: "IX_Shots_BoardId",
                table: "Shot",
                newName: "IX_Shot_BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Ships_ShipTypeId",
                table: "Ship",
                newName: "IX_Ship_ShipTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ships_BoardId",
                table: "Ship",
                newName: "IX_Ship_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shot",
                table: "Shot",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipType",
                table: "ShipType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ship",
                table: "Ship",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ship_Boards_BoardId",
                table: "Ship",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ship_ShipType_ShipTypeId",
                table: "Ship",
                column: "ShipTypeId",
                principalTable: "ShipType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shot_Boards_BoardId",
                table: "Shot",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }
    }
}
