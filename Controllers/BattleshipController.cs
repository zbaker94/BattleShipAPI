using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BattleShipAPI.Models;
using BattleShipAPI.Models.IncomingDTO;
using System.Formats.Tar;
using BattleShipAPI.Models.NonDB;

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

        // GET: api/Game/{id}
        [HttpGet ("Game")]
        public async Task<ActionResult<GameInstance>> GetGameInstanceById(int? id = null)
        {
            if(id == null)
            {
                return Ok(await _context.GameInstances.ToListAsync());
            }


            var gameInstance = await _context.GameInstances.FindAsync(id);

            if (gameInstance == null)
            {
                return NotFound();
            }

            return gameInstance;
        }

        // PUT: api/Game
        [HttpPut ("Game")]
        public async Task<IActionResult> UpdateGameInstance(GameInstance gameInstance)
        {
            _context.Entry(gameInstance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameInstanceExists(gameInstance.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Player
        [HttpPost ("Player")]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            _context.Player.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreatePlayer", new { id = player.Id }, player);
        }

        // GET: api/Player
        [HttpGet ("Player")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers(Guid? id)
        {
            if(id == null){
                return Ok(await _context.Player.ToListAsync());
            }

            var player = await _context.Player.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // PUT: api/Player
        [HttpPut ("Player")]
        public async Task<IActionResult> UpdatePlayer(Player player)
        {
            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(player.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Player
        [HttpDelete ("Player")]
        public async Task<ActionResult<Guid>> DeletePlayer(Guid id)
        {
            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Player.Remove(player);
            await _context.SaveChangesAsync();

            return player.Id;
        }

        // POST: api/StartGame
        [HttpPost ("StartGame")]
        public async Task<ActionResult<GameInstance>> StartGame(int id)
        {
            var gameInstance = await _context.GameInstances.FindAsync(id);

            if (gameInstance == null)
            {
                return NotFound();
            }

            if(gameInstance.Players.Length < 2)
            {
                return BadRequest(new { message = "Game must have at least 2 players to start", gameInstance });
            }

            gameInstance.Phase = GamePhase.Setup;

            await _context.SaveChangesAsync();


            return gameInstance;
        }

        // POST: api/PlaceShips
        [HttpPost ("PlaceShips")]
        public async Task<ActionResult<GameInstance>> PlaceShips(int boardId, [FromBody] PlaceShipsDTO[] ships)
        {
            var board = await _context.Boards.FindAsync(boardId);

            if (board == null)
            {
                return NotFound("Board not found");
            }

            if(board.Ships.Length > 0)
            {
                return BadRequest("Ships already placed on board");
            }

            var gameInstance = await _context.GameInstances.FindAsync(board.GameInstanceId);

            if (gameInstance == null)
            {
                return NotFound("Board not associated with a game instance");
            }

            if(gameInstance.Phase != GamePhase.Setup)
            {
                return BadRequest("Game must be in setup phase to place ships");
            }


            foreach (PlaceShipsDTO ship in ships)
            {
                var shipType = _context.ShipTypes.Where(t => t.Name.ToLower() == ship.ShipTypeName.ToLower()).FirstOrDefault();
                if(shipType == null)
                {
                    return BadRequest("Ship type not found for " + ship.ShipTypeName);
                }

                if(ship.Row < 0 || ship.Row >= board.Rows || ship.Column < 0 || ship.Column >= board.Columns)
                {
                    return BadRequest("Ship placement out of bounds");
                }

                if(shipType.Size < 1)
                {
                    return BadRequest("Ship size must be greater than 0");
                }

                // TODO prevent overlapping ships

                Ship newShip = new Ship(){
                    Row = ship.Row,
                    Column = ship.Column,
                    Vertical = ship.Vertical,
                    ShipType = shipType
                };

                board.Ships.Append(newShip);
            }

            await _context.SaveChangesAsync();

            return board.GameInstance;
        }

        // POST: api/EndSetup
        [HttpPost ("EndSetup")]
        public async Task<ActionResult<GameInstance>> EndSetup(int id)
        {
            var gameInstance = await _context.GameInstances.FindAsync(id);

            if (gameInstance == null)
            {
                return NotFound();
            }

            if(gameInstance.Phase != GamePhase.Setup)
            {
                return BadRequest("Game must be in setup phase to end setup");
            }

            // find empty boards for game instance
            var emptyBoards = _context.Boards
            .Where(b => b.GameInstanceId == id)
            .Where(b => b.Ships.Length == 0);

            if(emptyBoards.Count() > 0)
            {
                return BadRequest("All players must place ships before ending setup");
            }

            gameInstance.Phase = GamePhase.InProgress;

            gameInstance.ActivePlayerId = gameInstance.Players[0].Id;

            await _context.SaveChangesAsync();

            return gameInstance;
        }

        // POST: api/Shoot
        [HttpPost ("Shoot")]
        public async Task<ActionResult<GameInstance>> Shoot(int gameInstanceId, Guid offensivePlayerId, Guid targetPlayerId, int row, int column)
        {

            if(offensivePlayerId == Guid.Empty)
            {
                return BadRequest("Offensive player Id must not be empty");
            }

            if(targetPlayerId == Guid.Empty)
            {
                return BadRequest("Target player Id must not be empty");
            }

            if(offensivePlayerId == targetPlayerId)
            {
                return BadRequest("Offensive player and target player must be different");
            }

            var gameInstance = await _context.GameInstances.FindAsync(gameInstanceId);

            if (gameInstance == null)
            {
                return NotFound("Game instance not found");
            }

            if(gameInstance.Phase != GamePhase.InProgress)
            {
                return BadRequest("Game must be in progress to shoot");
            }

            if(gameInstance.Players.Where(p => p.Id == offensivePlayerId).Count() == 0)
            {
                return BadRequest("Offensive player not found in game instance");
            }

            if(gameInstance.Players.Where(p => p.Id == targetPlayerId).Count() == 0)
            {
                return BadRequest("Target player not found in game instance");
            }

            if(gameInstance.ActivePlayerId != offensivePlayerId)
            {
                return BadRequest("It is not this player's turn");
            }

            var targetBoard = _context.Boards
            .Where(b => b.GameInstanceId == gameInstanceId)
            .Where(b => b.Player.Id == targetPlayerId)
            .FirstOrDefault();

            if(targetBoard == null)
            {
                return NotFound("Target player not found");
            }

            if(row < 0 || row >= targetBoard.Rows || column < 0 || column >= targetBoard.Columns)
            {
                return BadRequest("Shot out of bounds");
            }

            var shot = new Shot(){
                Row = row,
                Column = column
            };

            var hit = false;

            foreach (Ship ship in targetBoard.Ships)
            {
                if(ship.Vertical)
                {
                    // if our coulmn matches and row is within the ship's size, hit is true
                    if(ship.Column == column && ship.Row <= row && row < ship.Row + ship.ShipType.Size)
                    {
                        hit = true;
                        break;
                    }
                }
                else
                {
                    // if our row matches and column is within the ship's size, hit is true
                    if(ship.Row == row && ship.Column <= column && column < ship.Column + ship.ShipType.Size)
                    {
                        hit = true;
                        break;
                    }
                }
            }

            shot.Hit = hit;
            targetBoard.BoardState.Append(shot);
            _context.Boards.Update(targetBoard);

            await _context.SaveChangesAsync();

            return gameInstance;
        }

        // POST: api/EndTurn
        [HttpPost ("EndTurn")]
        public async Task<ActionResult<GameInstance>> EndTurn(int gameInstanceId, Guid playerId)
        {
            var gameInstance = await _context.GameInstances.FindAsync(gameInstanceId);

            if (gameInstance == null)
            {
                return NotFound("Game instance not found");
            }

            if(gameInstance.Phase != GamePhase.InProgress)
            {
                return BadRequest("Game must be in progress to end turn");
            }

            if(gameInstance.Players.Where(p => p.Id == playerId).Count() == 0)
            {
                return BadRequest("Player not found in game instance");
            }

            if(gameInstance.ActivePlayerId != playerId)
            {
                return BadRequest("It is not this player's turn");
            }

            var playerIndex = Array.IndexOf(gameInstance.Players, gameInstance.Players.Where(p => p.Id == playerId).FirstOrDefault());

            if(playerIndex == gameInstance.Players.Length - 1)
            {
                gameInstance.ActivePlayerId = gameInstance.Players[0].Id;
            }
            else
            {
                gameInstance.ActivePlayerId = gameInstance.Players[playerIndex + 1].Id;
            }

            await _context.SaveChangesAsync();

            return gameInstance;
        }

        // GET api/GameState
        [HttpGet("GameState")]
        public ActionResult<List<string[,]>> GetBoardState(int[]? boardIds = null, int? gameId = null)
        {
            if ((boardIds == null || boardIds.Length == 0) && gameId == null)
            {
                return BadRequest("Must provide either boardId or gameId");
            }
            IQueryable<Board> boards = new List<Board>().AsQueryable();
            if (boardIds != null && boardIds.Length > 0)
            {
                boards = _context.Boards.Where(b => boardIds.Contains(b.Id));
            }
            else if (gameId != null)
            {
                if (!GameInstanceExists((int)gameId))
                {
                    return NotFound("Game Instance not found");
                }
                boards = _context.Boards.Where(b => b.GameInstanceId == gameId);
            }

            if (boards.Count() == 0)
            {
                return NotFound("No boards found for game instance");
            }

            // populate board array with board state and ships
            var result = new List<string[,]>();
            foreach (Board board in boards)
            {
                var gameBoardArray = new GameBoard(board).ToArray();
                result.Add(gameBoardArray);
            }

            return Ok(result);
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
