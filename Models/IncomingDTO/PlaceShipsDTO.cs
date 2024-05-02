namespace BattleShipAPI.Models.IncomingDTO;

public class PlaceShipsDTO
{
    public required string ShipTypeName { get; set; }
    public required int Row { get; set; }
    public required int Column { get; set; }

    public required bool Vertical { get; set; }
}