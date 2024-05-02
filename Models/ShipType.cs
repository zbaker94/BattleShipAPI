namespace BattleShipAPI.Models;

public class ShipType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Size { get; set; }
}