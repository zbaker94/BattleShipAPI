using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShipAPI.Models;

public enum GamePhase
{
    NotStarted,
    Setup,
    InProgress,
    Finished
}

public class GameInstance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    public GamePhase Phase { get; set; }

    public required Player[] Players { get; set; }

    public Guid? WinnerId { get; set; }

}