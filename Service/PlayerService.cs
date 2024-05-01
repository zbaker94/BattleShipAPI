using BattleShipAPI.Models;

namespace BattleShipAPI.Service;

public class PlayerService
{
    private readonly BattleshipContext _context;

    public PlayerService(BattleshipContext context)
    {
        _context = context;
    }

    public async Task<Player> GetPlayer(Guid id)
    {
        var player = await _context.Player.FindAsync(id);

        if (player == null)
        {
            throw new Exception("Player not found with id: " + id);
        }
        return player;
    }
}