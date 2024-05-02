using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShipAPI.Models;

public class Ship
{
    public int Id { get; set; }
    public int ShipTypeId { get; set; }
    [ForeignKey("ShipTypeId")]
    public required ShipType ShipType { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public bool Vertical { get; set; }
}