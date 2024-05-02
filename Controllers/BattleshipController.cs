using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BattleShipAPI.Models;

namespace BattleShipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase
    {
        private readonly BattleshipContext _context;

        public BattleshipController(BattleshipContext context)
        {
            _context = context;
        }

        
        [HttpPost ("Game")]
        public async Task<ActionResult<GameInstance>> CreateGame([FromBody]Player[] players)
        {
            if(players.Length > 4)
            {
                return BadRequest("Game may not have more than 4 players");
            }

            // create game instance
            foreach (Player player in players)
            {   
                if(player.Id == Guid.Empty)
                {
                    return BadRequest("all Player Ids must not be empty");
                }
                
                if(!PlayerExists(player.Id))
                {
                    return BadRequest("all Players Must Already Exist");
                }

            }

            if(players.Length == 0)
            {
                players = new Player[4];
            }

            GameInstance gameInstance = new GameInstance(){
                Players = players,
                Phase = GamePhase.NotStarted
            };

            _context.GameInstances.Add(gameInstance);

            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateGame", new { id = gameInstance.Id }, gameInstance);

        }

        // POST api/AddPlayerToGame
        [HttpPost ("AddPlayerToGame")]
        public async Task<ActionResult<GameInstance>> AddPlayerToGame(int gameId, Guid playerId)
        {

            if(playerId == Guid.Empty)
            {
                return BadRequest("Player Id must not be empty");
            }

            if(!PlayerExists(playerId))
            {
                return BadRequest("Player Must Already Exist");
            }

            var player = await _context.Player.FindAsync(playerId);

            if(!GameInstanceExists(gameId))
            {
                return NotFound("Game Instance not found");
            }
            var gameInstance = await _context.GameInstances.FindAsync(gameId);

            if(gameInstance == null)
            {
                return NotFound("Game Instance not found");
            }

            if(gameInstance.Players.Contains(player))
            {
                return BadRequest("Player already in game");
            }

            if(gameInstance.Players.Length >= 4)
            {
                return BadRequest("Game may not have more than 4 players");
            }

            gameInstance.Players.Append(player);

            await _context.SaveChangesAsync();

            return Ok(gameInstance);
        }

        // DELETE api/RemovePlayerFromGame
        [HttpDelete ("RemovePlayerFromGame")]
        public async Task<ActionResult<GameInstance>> RemovePlayerFromGame(Guid gameId, Guid playerId)
        {
            if(playerId == Guid.Empty)
            {
                return BadRequest("Player Id must not be empty");
            }

            if(!PlayerExists(playerId))
            {
                return BadRequest("Player Must Already Exist");
            }

            var player = await _context.Player.FindAsync(playerId);

            var gameInstance = await _context.GameInstances.FindAsync(gameId);

            if(gameInstance == null)
            {
                return NotFound("Game Instance not found");
            }

            if(!gameInstance.Players.Contains(player))
            {
                return BadRequest("Player not in game");
            }

            gameInstance.Players = gameInstance.Players.Where(p => p.Id != playerId).ToArray();

            await _context.SaveChangesAsync();

            return Ok(gameInstance);
        }

        // DELETE api/Game
        [HttpDelete ("Game")]
        public async Task<ActionResult<GameInstance>> DeleteGame([FromQuery]int id)
        {
            var gameInstance = await _context.GameInstances.FindAsync(id);
            if (gameInstance == null)
            {
                return NotFound();
            }

            _context.GameInstances.Remove(gameInstance);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private bool PlayerExists(Guid id)
        {
            return _context.Player.Any(e => e.Id == id);
        }

        private bool GameInstanceExists(int id)
        {
            return _context.GameInstances.Any(e => e.Id == id);
        }
    }
}
