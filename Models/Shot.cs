
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShipAPI.Models;

public class Shot
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
    public int Id { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public bool Hit { get; set; }
}