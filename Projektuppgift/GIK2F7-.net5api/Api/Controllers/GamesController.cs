using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("games")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesRepository repository;
        private readonly ILogger<GamesController> logger;

        public GamesController(IGamesRepository repository, ILogger<GamesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// Search for games inside database
        /// </summary>
        /// <returns>Game dto</returns>
        // GET /games
        [HttpGet]
        public async Task<IEnumerable<GameDto>> GetGamesAsync(string name = null)
        {
            var games = (await repository.GetGamesAsync())
                        .Select(game => game.AsDto());

            if (!string.IsNullOrWhiteSpace(name))
            {
                games = games.Where(game => game.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {games.Count()} games");

            return games;
        }
        
        /// <summary>
        /// Create game inside database
        /// </summary>
        /// <returns>Http status code</returns>
        // POST /games
        [HttpPost]
        [ActionName(nameof(GetGameAsync))]
        public async Task<ActionResult<GameDto>> CreateGameAsync(CreateGameDto gameDto)
        {
            Game game = new()
            {
                //Id = int.Newint(),
                Name = gameDto.Name,
                Description = gameDto.Description,
                Rating = gameDto.Rating,
                Image = gameDto.Image
            };

            await repository.CreateGameAsync(game);

            return CreatedAtAction(nameof(GetGameAsync), new { id = game.Id }, game.AsDto());
        }

        /// <summary>
        /// Update game inside database
        /// </summary>
        /// <param name="id">Id to update</param>
        /// <param name="gameDto">Game dto</param>
        /// <returns>Http status code</returns>
        // PUT /games/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGameAsync(int id, UpdateGameDto gameDto)
        {
            var existingGame = await repository.GetGameAsync(id);

            if (existingGame is null)
            {
                return NotFound();
            }

            existingGame.Name = gameDto.Name;
            existingGame.Description = gameDto.Description;
            existingGame.Rating = gameDto.Rating;
            existingGame.Image = gameDto.Image;

            await repository.UpdateGameAsync(existingGame);

            return NoContent();
        }

        /// <summary>
        /// Get database data from index id
        /// </summary>
        /// <param name="id">index id</param>
        /// <returns>game dto</returns>
        // GET /games/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGameAsync(int id)
        {
            var game = await repository.GetGameAsync(id);

            if (game is null)
            {
                return NotFound();
            }

            return game.AsDto();
        }

        /// <summary>
        /// Deletes data from database
        /// </summary>
        /// <param name="id">index id</param>
        /// <returns>http status code</returns>
        // DELETE /games/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGameAsync(int id)
        {
            var existingGame = await repository.GetGameAsync(id);

            if (existingGame is null)
            {
                return NotFound();
            }

            await repository.DeleteGameAsync(id);

            return NoContent();
        }
    }
}