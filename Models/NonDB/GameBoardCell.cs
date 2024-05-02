namespace BattleShipAPI.Models.NonDB;

public class GameBoardCell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public Shot? Shot { get; set; }
    public required Ship? Ship { get; set; }
}