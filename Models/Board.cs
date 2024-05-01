using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShipAPI.Models;

public class Board
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int GameInstanceId { get; set; }

    public required GameInstance GameInstance { get; set; }

    public required Player Player { get; set; }

    public int Rows { get; set; }
    public int Columns { get; set; }

    public required Shot[] BoardState { get; set; }
}