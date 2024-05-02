using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedShipType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[ShipType] ([Name], [Size]) VALUES (N'Destroyer', '2'), (N'Cruiser', '3'), (N'Battleship', '4'), (N'Aircraft Carrier', '5');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        
    }
}
